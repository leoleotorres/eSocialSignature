unit eSocialSignature;

interface

type
  TESocialSignature = class
    class procedure SignSHA256Ansi(var AXml: PAnsiChar; ANodeToSign: PAnsiChar; ASerialNumber: PAnsiChar; APassword: PAnsiChar);
    class procedure SignSHA256Unicode(var AXml: PChar; ANodeToSign: PChar; ASerialNumber: PChar; APassword: PChar);
  end;

implementation

uses
  Windows, SysUtils;

const
  DLLNAME = 'eSocialSignature.dll';

{
procedure SignSHA256Ansi(
  var xml: PAnsiChar;
  nodeToSign: PAnsiChar;
  certificateSerialNumber: PAnsiChar;
  certificatePassword: PAnsiChar); stdcall;

procedure SignSHA256Unicode(
  var xml: PChar;
  nodeToSign: PChar;
  certificateSerialNumber: PChar;
  certificatePassowrd: PChar); stdcall;
}


{ TESocialSignatureWrapper }

class procedure TESocialSignature.SignSHA256Ansi(var AXml: PAnsiChar; ANodeToSign: PAnsiChar; ASerialNumber: PAnsiChar; APassword: PAnsiChar);
type
  TProc = procedure(var AXml: PAnsiChar; ANodeToSign: PAnsiChar; ASerialNumber: PAnsiChar; APassword: PAnsiChar); stdcall;
var
  dllHandle: THandle;
  proc: TProc;
begin
  dllHandle := LoadLibrary('eSocialSignature.dll');
  if dllHandle < HINSTANCE_ERROR then
  begin
    raise Exception.Create('N�o foi poss�vel encontrar a DLL ' + DLLNAME + '.' + #13 + SysErrorMessage(GetLastError));
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

class procedure TESocialSignature.SignSHA256Unicode(var AXml: PChar; ANodeToSign: PChar; ASerialNumber: PChar; APassword: PChar);
type
  TProc = procedure(var AXml: PChar; ANodeToSign: PChar; ASerialNumber: PChar; APassword: PChar); stdcall;
var
  dllHandle: THandle;
  proc: TProc;
begin
  dllHandle := LoadLibrary('eSocialSignature.dll');
  if dllHandle < HINSTANCE_ERROR then
  begin
    raise Exception.Create('N�o foi poss�vel encontrar a DLL ' + DLLNAME + '.' + #13 + SysErrorMessage(GetLastError));
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

end.
