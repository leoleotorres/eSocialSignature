program DelphiTest;

{$APPTYPE CONSOLE}

{$R *.res}

uses
  {$if CompilerVersion > 20 }
  System.SysUtils,
  System.Classes,
  {$else}
  SysUtils,
  Classes,
  {$ifend }
  eSocialSignature in 'eSocialSignature.pas';

var
  directory: string;
  fileName: string;
  fileXml: TStringList;
{$ifdef UNICODE}
  xmlUnicode: PChar;
{$else}
  xmlAnsi: PAnsiChar;
{$endif}

  CertTokenA3SerialNumber: string;
  CertTokenA3Pin: string;

begin
  fileXml := TStringList.Create;
  try
    try
      CertTokenA3SerialNumber := GetEnvironmentVariable('CERT_TOKEN_A3_SERIAL_NUMBER');
      CertTokenA3Pin := GetEnvironmentVariable('CERT_TOKEN_A3_PIN'); 

      directory := ExtractFilePath(ParamStr(0));
      fileName := directory + 'envio-sem-assinatura.xml';

(*
{$ifdef UNICODE}
      xmlUnicode := PChar(fileXml.Text);
{$else}
      xmlAnsi := PAnsiChar(AnsiString(fileXml.Text));
{$endif}

{$ifdef UNICODE}
      TESocialSignature.SignSHA256Unicode(xmlUnicode, 'evtInfoEmpregador', SERIAL_NUMBER, PASSWORD);
      Writeln('Unicode:');
      Writeln('');
      Writeln(UnicodeString(xmlUnicode));
{$else}
      TESocialSignature.SignSHA256Ansi(xmlAnsi, 'evtInfoEmpregador', SERIAL_NUMBER, PASSWORD);
      Writeln('ANSI:');
      Writeln('');
      Writeln(AnsiString(xmlAnsi));
{$endif}
*)

      TESocialSignature.SignFileWithSHA256Ansi(
        PAnsiChar(AnsiString(fileName)),
        PAnsiChar(AnsiString('evtInfoEmpregador')),
        PAnsiChar(AnsiString(CertTokenA3SerialNumber)),
        PAnsiChar(AnsiString(CertTokenA3Pin)));

      fileXml.LoadFromFile(fileName);
      Writeln(fileXml.Text);        

      Writeln('');
    except
      on E: Exception do
        Writeln(E.ClassName, ': ', E.Message);
    end;
  finally
    fileXml.Free;
    Writeln('');
    Write('Processo finalizado! Pressione ENTER para sair... ');
    Readln;
  end;
end.

