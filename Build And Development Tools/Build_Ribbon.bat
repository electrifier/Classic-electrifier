::
:: 
::  electrifier
:: 
::  Copyright 2017-19 Thorsten Jung, www.electrifier.org
::  
::  Licensed under the Apache License, Version 2.0 (the "License");
::  you may not use this file except in compliance with the License.
::  You may obtain a copy of the License at
::  
::      http://www.apache.org/licenses/LICENSE-2.0
::  
::  Unless required by applicable law or agreed to in writing, software
::  distributed under the License is distributed on an "AS IS" BASIS,
::  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
::  See the License for the specific language governing permissions and
::  limitations under the License.
::
::

@ECHO off

CLS

CD "S:\[Git.Workspace]\[electrifier.Workspace]\electrifier\src\electrifier.Core\Resources"

IF EXIST "ElApplicationWindow.Ribbon.xml" CALL :BuildRibbonResource "ElApplicationWindow.Ribbon.xml"

PAUSE

EXIT /B 0

:: FUNCTION BuildRibbonResource

:BuildRibbonResource

ECHO Found %~1, Building Resource File...

rgc %~1

