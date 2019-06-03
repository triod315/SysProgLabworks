from winreg import *

keyVal=r'Software\Hryshchuk'

key=CreateKey(HKEY_LOCAL_MACHINE,keyVal)

SetValueEx(key,'P1',0,REG_SZ,'Спеціальність КІ')
SetValueEx(key,'P2',0,REG_DWORD,0x2A4BCEDE)
SetValueEx(key,'P3',0,REG_DWORD,709611230)