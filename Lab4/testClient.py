from xml.dom import minidom

import xml.etree.ElementTree as et

import socket

import datetime

def writeLog(text, cons=True):
    
    if cons:
        print(text)
    text="\n"+f"{datetime.datetime.now()}"+"\n"+text
    file=open('log.txt','a')
    file.write(text)
    

def analResp(response):
    root = et.fromstring(response)    
    writeLog(root[0].text,False)
    return root[1].text

def sendRequest(ip, typeR,currKey,newKey):

    #root element
    rtype=et.Element(typeR)

    currKeyElem=et.SubElement(rtype,"currentkey")
    newKeyElem=et.SubElement(rtype,'newkey')
    currKeyElem.text=currKey
    newKeyElem.text=newKey

    mydata=et.tostring(rtype, encoding="unicode")
    reqfile=open("request.xml","w")
    reqfile.write(mydata)

    
    clientSock = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
    #clientSock.bind(('192.168.0.101', 54000))
    server_addres=(ip,54000)
    sent=clientSock.sendto(mydata.encode(),server_addres)

    #print('waiting to receive')
    data, server = clientSock.recvfrom(54000)
    #print(f'received \n{data.decode("utf-8")}')
    
    return data.decode("utf-8")


def main():
    ip=input("Input server ip (192.168.0.101 default):")
    if ip=='':
        ip="192.168.0.101"

    
    while True:
        command=input(f"rrd {ip}> ")
        if command=='create':
            key=input('current key:')
            newkey=input('new key:')
            textresp=sendRequest(ip,'request',key,newkey)
            code=analResp(textresp)
            if code=='404':
                writeLog(f'key "{key}" doesnt exist')
            elif code=='300':
                writeLog(f'key "{newkey}" already exist')
            elif code=='400':
                answ=input(f'Create new sub key "{newkey}" ?[y/n]:')
                if answ=='y':
                    textresp=sendRequest(ip,'create',key,newkey)
                    if analResp(textresp)=='200':
                        writeLog(f'key "{key}/{newkey} was created"')
        elif command=='exit':
            return


if __name__ == "__main__":
    main()



