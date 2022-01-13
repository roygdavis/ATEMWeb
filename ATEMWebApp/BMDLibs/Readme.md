# How to import the Atem BMDSwitcherAPI.idl into this project
1. Add C++ tools to Visual Studio if you don't already have them
2. midl ./BMDSwitcherAPI.idl
3. This will generate a BMDSwitcher.tlb file
4. Now convert to a dll:
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\tlbimp.exe" BMDSwitcherAPI.tlb /out:BMDSwitcherAPI.dll