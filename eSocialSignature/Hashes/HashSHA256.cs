using System;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using eSocialSignature.Extensions;
using eSocialSignature.Log;
using eSocialSignature.Repository;
using Security.Cryptography;

namespace eSocialSignature.Hashes
{
    public class HashSHA256
    {
        private readonly Logger _log;

        public HashSHA256(Logger log)
        {
            _log = log;
        }

        public string Sign(string xml, string nodeToSign, string certificateSerialNumber, string certificatePassword = null)
        {
            try
            {
                _log.Debug("");
                _log.Debug("NOVA ASSINATURA");
                _log.Debug(new string('-', 150));
                _log.Debug("");
                _log.Debug($"O conteúdo do XML é NULO ou Vazio? {string.IsNullOrEmpty(xml)}");
                _log.Debug("");
                _log.Debug($"nodeToSign: {nodeToSign}");
                _log.Debug($"certificateSerialNumber: {certificateSerialNumber}");
                _log.Debug($"certificatePassword: {certificatePassword}");
                _log.Debug("");
                _log.Debug($"XML Recebido:{Environment.NewLine}{Environment.NewLine}{xml}");
                _log.Debug("");

                if (string.IsNullOrEmpty(xml))
                    throw new Exception("Conteúdo de XML inválido");

                xml = xml.NormalizeXml();

                if (string.IsNullOrEmpty(xml))
                    throw new Exception("O conteúdo do XML não foi informado");

                var doc = new XmlDocument();
                try
                {
                    doc.PreserveWhitespace = true;
                    doc.LoadXml(xml);
                }
                catch (Exception e)
                {
                    _log.Error(e, "Erro ao carregar o Documento XML");
                    throw;
                }

                _log.Debug("Documento XML criado");

                var nodes = doc.GetElementsByTagName(nodeToSign);
                if (nodes.Count == 0)
                    throw new Exception("Conteúdo de XML inválido");

                _log.Debug($"Tag {nodeToSign} encontrada");

                var certificate = new CertificateRepository().GetBySerialNumber(certificateSerialNumber);
                if (certificate == null)
                    throw new Exception("Não foi possível encontrar o certificado");

                _log.Debug($"Certificado obtido: {certificate.Subject}");

                foreach (XmlElement node in nodes)
                {
                    _log.Debug("Adicionar tipo de criptografia a engine");

                    CryptoConfig.AddAlgorithm(typeof(RSAPKCS1SHA256SignatureDescription), "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");

                    _log.Debug("RSAPKCS1SHA256SignatureDescription adicionada");

                    var keyInfo = new KeyInfo();
                    keyInfo.AddClause(new KeyInfoX509Data(certificate));

                    _log.Debug("KeyInfo criado e cláusula adicionada");

                    var Key = (RSACryptoServiceProvider)certificate.PrivateKey;

                    _log.Debug("key obtida");

                    var signedXml = new SignedXml(node)
                    {
                        SigningKey = Key,
                        KeyInfo = keyInfo
                    };

                    _log.Debug("SignedXML criado");

                    signedXml.SigningKey = ReadCard(Key, certificatePassword);
                    signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
                    signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

                    _log.Debug("SignedXML preparado");

                    var id = node.Attributes.GetNamedItem("Id").InnerText;

                    _log.Debug($"ID #{id}");

                    var reference = new Reference($"#{id}");
                    reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                    reference.AddTransform(new XmlDsigC14NTransform(false));
                    reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
                    reference.Uri = "";

                    _log.Debug("Referências criadas");

                    signedXml.AddReference(reference);

                    _log.Debug("Referências adicionadas");

                    _log.Debug("A criar assinatura");

                    signedXml.ComputeSignature();

                    _log.Debug("Assinatura criada");

                    var signature = signedXml.GetXml();

                    _log.Debug("A adicionar a assinatura no documento");

                    var eSocialNode = node.ParentNode;
                    if (eSocialNode == null)
                        throw new Exception("Não foi possível encontrar o Nó do eSocial");

                    eSocialNode.AppendChild(signature);

                    _log.Debug("Assinatura adicionada");
                }

                _log.Debug("Atualizando XML de saída");

                var sb = new StringBuilder();
                using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true }))
                    doc.WriteTo(writer);

                _log.Debug($"XML Assinado:{Environment.NewLine}{Environment.NewLine}{sb}{Environment.NewLine}");

                var signatureCount = doc.GetElementsByTagName("Signature").Count;
                _log.Debug($"Quantidade de assinaturas geradas: {signatureCount}");

                return doc.OuterXml;
            }
            catch (Exception e)
            {
                _log.Debug("Erro");
                _log.Error(e, "");
                throw;
            }
        }

        private static RSACryptoServiceProvider ReadCard(RSACryptoServiceProvider key, string cardPin = null)
        {
            var csp = new CspParameters(key.CspKeyContainerInfo.ProviderType, key.CspKeyContainerInfo.ProviderName);
            var ss = new SecureString();

            foreach (var a in cardPin ?? "")
                ss.AppendChar(a);

            csp.ProviderName = key.CspKeyContainerInfo.ProviderName;
            csp.ProviderType = key.CspKeyContainerInfo.ProviderType;
            csp.KeyNumber = key.CspKeyContainerInfo.KeyNumber == KeyNumber.Exchange ? 1 : 2;
            csp.KeyContainerName = key.CspKeyContainerInfo.KeyContainerName;

            if (ss.Length > 0)
            {
                csp.KeyPassword = ss;
                csp.Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseDefaultKeyContainer;
            }
            else
            {
                csp.Flags = CspProviderFlags.UseDefaultKeyContainer;
            }

            var rsa = new RSACryptoServiceProvider(csp);
            return rsa;
        }
    }
}