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

const
  SERIAL_NUMBER = '41dacd08daa347b1a000d83eb510f4a3';
  PASSWORD = '1234';

begin
  fileXml := TStringList.Create;
  try
    try
      directory := ExtractFilePath(ParamStr(0));
      fileName := directory + 'envio-sem-assinatura.xml';
      fileXml.LoadFromFile(fileName);

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

