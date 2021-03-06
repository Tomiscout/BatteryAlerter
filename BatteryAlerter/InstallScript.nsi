; #######################################################################################
; # This NSIS script was generated by Visual & Installer for MS Visual Studio           #
; # Visual & Installer (c) 2012 - 2016 unSigned, s. r. o. All Rights Reserved.          #
; # Visit http://www.visual-installer.com/ for more details.                            #
; #######################################################################################

; NSIS Modern User Interface

; Include Modern UI
!include "MUI2.nsh"

; General

  ; Name and file
  Name "Battery Alerter"
  OutFile "BatteryAlerter.exe"

  ; Default installation folder
  InstallDir "$LOCALAPPDATA\Battery Alerter"
  
  ; Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\BatteryAlerter" ""

  ; Request application privileges for Windows Vista/7/8/10
  RequestExecutionLevel user

; --------------------------------
; Interface Settings

  !define MUI_ABORTWARNING

; --------------------------------
; Pages

  !insertmacro MUI_PAGE_LICENSE "${NSISDIR}\Docs\Modern UI\License.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
; --------------------------------
; Languages
 
  !insertmacro MUI_LANGUAGE "English"

; --------------------------------
; Installer Sections

Section "Battery Alerter" SecDummy

  SetOutPath "$INSTDIR"
  
  File "BatteryAlert.exe"
  File "BatteryAlert.exe.config"
  File "Interop.WMPLib.dll"
  File /r "sounds"

  createShortCut "$SMPROGRAMS\Battery Alerter.lnk" "$INSTDIR\BatteryAlert.exe"
  createShortCut "$DESKTOP\Battery Alerter.lnk" "$INSTDIR\BatteryAlert.exe"

  ; Store installation folder
  WriteRegStr HKCU "Software\BatteryAlerter" "" $INSTDIR
  
  ; Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

; --------------------------------
; Descriptions

  ; Language strings
  LangString DESC_SecDummy ${LANG_ENGLISH} "Alerts battery by alerting alert of battery-er."

  ; Assign language strings to sections
  !insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
    !insertmacro MUI_DESCRIPTION_TEXT ${SecDummy} $(DESC_SecDummy)
  !insertmacro MUI_FUNCTION_DESCRIPTION_END

; --------------------------------
; Uninstaller Section

Section "Uninstall"

  ; ADD YOUR OWN FILES HERE...
  ;Settings 
  RMDir /r "$LOCALAPPDATA\BatteryAlert"

  Delete "$INSTDIR\BatteryAlert.exe"
  Delete "$INSTDIR\BatteryAlert.exe.config"
  Delete "$INSTDIR\Interop.WMPLib.dll"
  RMDir /r "$INSTDIR\sounds"

  Delete "$SMPROGRAMS\Battery Alerter.lnk"
  Delete "$DESKTOP\Battery Alerter.lnk"

  Delete "$INSTDIR\Uninstall.exe"

  RMDir "$INSTDIR"

  DeleteRegKey /ifempty HKCU "Software\BatteryAlerter"

SectionEnd
