import urllib.request
import sys
import os

def checkStringUpper(text, sample):
    for txt in text:
        if txt.upper() == sample.upper():
            return True
    return False

def splitString(str, characters):
    result=[]
    tmpStr=''
    for ch in str:
        if ch in characters:
            result.append(tmpStr)
            tmpStr=''
        else:
            tmpStr+=ch
    return result

try:
    url='http://mail.univ.net.ua/manual.txt'

    file_name='manual.txt'

    light_file_name='Manual-LIGHT.txt'

    print(f"string to remove: 'chip' or '{sys.argv[1]}'\nstring to replace by MODIFIED: '{sys.argv[2]}'")

    with urllib.request.urlopen(url) as response, open(file_name, 'w') as out_file:
        data = response.read().decode('utf-8') 
        out_file.write(data)

    lines=data.splitlines()

    with open(light_file_name,'w') as out_file:
        for line in lines:
            words=splitString(line,[' ','!','.',',',':'])
            if checkStringUpper(words,'chip') or checkStringUpper(words,sys.argv[1]):
                continue
    
            if sys.argv[2] in words:
                line="MODIFIED"

            out_file.write(line+os.linesep)
except:
    print(':(\nSomething was wrong')
