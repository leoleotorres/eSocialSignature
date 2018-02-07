# eSocialSignature

DLL para assinatura do XML do eSocial com Certificado A3 via HASH SHA256.

## Detalhes

- DLL escrita em C# para ser usada como uma DLL comum escrita em C;
- Escrita em .NET 4.0 para compatibilidade com Windows XP;
- Baseada na **[CertFly](https://github.com/leivio/CertFly)**;
- Não requer trocar de arquivos via disco rídigo;

## Como usar

- Há duas DLL´s que precisam ser copiadas para a pasta do executável do seu projeto, a eSocialSignature.dll e a Security.Cryptography.dll. Ambas podem ser encontradas na pasta do projeto de exemplo em **[Delphi](https://github.com/tiagopsilva/eSocialSignature/tree/master/DelphiTest)**;

- Há dois métodos, sendo que `SignSHA256Ansi` é específico para linguagens não-unicode (no caso do Delphi são as versões inferiores a 2009);

- Ambas podem ser usadas em linguages com Unicode, desde que seja usado da maneira correta, sendo `SignSHA256Unicode` a indicada para essas linguages (no caso do Delphi são as versões 2009 e superiores);

- Caso deseje usar com Delphi você pode copiar o arquivo **eSocialSignature.pas** na pasta de exemplo em **[Delphi](https://github.com/tiagopsilva/eSocialSignature/tree/master/DelphiTest)**;

- Caso contrário você pode fazer referência aos métodos diretamente, ou carregar dinamicamente (recomendado):

Referência direta:
```
procedure SignSHA256Ansi(
  var xml: PAnsiChar;
  nodeToSign: PAnsiChar;
  certificateSerialNumber: PAnsiChar;
  certificatePassword: PAnsiChar); stdcall; external 'eSocialSignature.dll'; 

procedure SignSHA256Unicode(
  var xml: PChar;
  nodeToSign: PChar;
  certificateSerialNumber: PChar;
  certificatePassowrd: PChar); stdcall; external 'eSocialSignature.dll'; 
```

Referência dinâmica:
```
procedure SignSHA256Ansi(var AXml: PAnsiChar; ANodeToSign: PAnsiChar; 
  ASerialNumber: PAnsiChar; APassword: PAnsiChar);
type
  TProc = procedure(var AXml: PAnsiChar; ANodeToSign: PAnsiChar; 
      ASerialNumber: PAnsiChar; APassword: PAnsiChar); stdcall;
var
  dllHandle: THandle;
  proc: TProc;
begin
  dllHandle := LoadLibrary('eSocialSignature.dll');
  if dllHandle < HINSTANCE_ERROR then
  begin
    raise Exception.Create('Não foi possível encontrar a DLL ' + DLLNAME + '.' +#13+ 
        SysErrorMessage(GetLastError));
  end;
  try
    @proc := GetProcAddress(dllHandle, 'SignSHA256Ansi');
    if Assigned(@proc) then
    begin
      proc(AXml, ANodeToSign, ASerialNumber, APassword);
    end;
  finally
    FreeLibrary(dllHandle);
  end;
end;

procedure SignSHA256Unicode(var AXml: PChar; ANodeToSign: PChar; ASerialNumber: PChar; APassword: PChar);
type
  TProc = procedure(var AXml: PChar; ANodeToSign: PChar; ASerialNumber: PChar; APassword: PChar); stdcall;
var
  dllHandle: THandle;
  proc: TProc;
begin
  dllHandle := LoadLibrary('eSocialSignature.dll');
  if dllHandle < HINSTANCE_ERROR then
  begin
    raise Exception.Create('Não foi possível encontrar a DLL ' + DLLNAME + '.' +#13+ 
        SysErrorMessage(GetLastError));
  end;
  try
    @proc := GetProcAddress(dllHandle, 'SignSHA256Unicode');
    if Assigned(@proc) then
    begin
      proc(AXml, ANodeToSign, ASerialNumber, APassword);
    end;
  finally
    FreeLibrary(dllHandle);
  end;
end;
```

- Para usar deve-se:  
1) passar o texto do XML para uma variável do PAnsiChar ou PChar, conforme plataforma utilizada, utilizando o devido cast;
2) passar a variável para o devido método de assinatura com os demais parâmetros;
3) usar a mesma variável com o XML já assinado;
4) Dos métodos de assinatura os parâmetros são:

```
procedure SignSHA256Unicode(         // ou SignSHA256Ansi
    var xml: PChar;                  // Conteúdo do arquivo XML
    nodeToSign: PChar;               // Tag/Node XML que será assinado
    certificateSerialNumber: PChar;  // Número Serial do Certificado que será usado
    certificatePassowrd: PChar);     // Pin do Certificado
```

### Exemplo com a eSocialSignature.pas e plataforma com strings em ANSI:

```
var
  xml: PAnsiChar;
begin
  xml := PAnsiChar(AnsiString(xmlDoc.XML.Text));
  TESocialSignature.SignSHA256Unicode(xml, 'evtInfoEmpregador', 'eaee2da6eabd4e0aa211e2a18e7c749c', '1234');
  xmlDoc.LoadXml(xml);
end;
```

### Exemplo com a eSocialSignature.pas e plataforma com strings em Unicode:

```
var
  xml: PChar;
begin
  xml := PChar(xmlDoc.XML.Text);
  TESocialSignature.SignSHA256Unicode(xml, 'evtInfoEmpregador', 'eaee2da6eabd4e0aa211e2a18e7c749c', '1234');
  xmlDoc.LoadXml(xml);
end;
```

## LICENÇA:

MIT License

Copyright (c) 2018 Tiago Silva

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
