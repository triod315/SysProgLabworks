#!/usr/bin/python3.6
import os
import requests
from time import sleep
from datetime import datetime
import sys
import configparser
import smtplib
from email.message import EmailMessage
import re
import telebot


logFile='swd.log'

httpCode = 0
init = False
shutdownAllow = False
wanted=True
wantedNumber=True

#method for wirting logs
def writeLog(path, mess):
    with open(path, 'a') as file:
        file.write(f"{datetime.now().strftime('%Y-%m-%d %H:%M:%S')} :: {mess}\n")

###########################################################################
#                       Telegram bot part                                 #
###########################################################################


chNumberFlag=False
chURLFlag=False
chWordFlag=False

def writeDTF(dicioary,filename):
    file=open(filename,"w")
    for key in dicioary:
        file.write(f"{key} {dicioary[key]}\n")
    file.close()

def readFTD(filename):
    dictionary={}
    file=open(filename,"r")
    lines=file.readlines()
    file.close()
    for line in lines:
        ln=line.split()
        dictionary[ln[0]]=int(ln[1])
        #print(f"{ln[0]} {ln[1]}\n")
    return dictionary

telUsers={}

TeleToken='719454765:AAE_Yr536HQ4gl_cuJJKnIszqp_z5Jnabc0'

bot=telebot.TeleBot(TeleToken)


@bot.message_handler(commands=["start"])
def start(message):
    bot.send_message(message.chat.id, "You have been selected")
    writeLog(logFile,f"id: {id} \t username: {message.chat.username} selected")
    addUser(message.chat.id,message.chat.username)
    #bot.stop_polling()

def addUser(id,username):
    if not id in telUsers:
        telUsers[username]=id
        writeDTF(telUsers,"tusers.txt")
        writeLog(logFile,f"id: {id} \t username: {username} added")

def broadcastSend(mess):
    print(mess)
    for user in telUsers:
        sendTelegramMessage(mess,user) 

@bot.message_handler(commands=["showconfig"])
def showconfig(message):
    mess=""
    file=open("settings.ini","r+")
    for str in file.readlines():
        mess=mess+str
    for user in telUsers:
        sendTelegramMessage(mess,user) 

@bot.message_handler(commands=["churl"])
def changeURL(message):
    global chURLFlag
    broadcastSend("Write new URL")
    chURLFlag=True

@bot.message_handler(commands=["chword"])
def changeWord(message):
    global chWordFlag
    broadcastSend("Write new wanted words")
    chWordFlag=True

@bot.message_handler(commands=["chnumber"])
def changeNumber(message):
    global chNumberFlag
    broadcastSend("Write new wanted number")
    chNumberFlag=True

@bot.message_handler(content_types=['text'])
def changeDConfig(message):
    global chURLFlag
    global chWordFlag
    global chNumberFlag
    param=message.text
    cfg = configparser.ConfigParser()
    cfg.read("settings.ini")
    if chURLFlag:
        global source
        cfg.set("Settings","Source",param)
        source=param
        writeLog(logFile,f'source changed to "{source}"')
        chURLFlag=False
    if chNumberFlag:
        global wantedNuber
        cfg.set("Settings","WantedNumber",param)
        wantedNuber=param
        writeLog(logFile,f'wanted number was changed to "{wantedNuber}"')
        chNumberFlag=False
    if chWordFlag:
        global wantedWord
        cfg.set("Settings","WantedWord",param)
        wantedWord=param
        writeLog(logFile,f'wanted words was changed to "{wantedWord}"')
        chWordFlag=False
    with open('settings.ini','w') as configFile:
        cfg.write(configFile)
        writeLog(logFile,'Daemon configuration file was changed')
def sendTelegramMessage(message, usraname):
    if usraname in telUsers:
        bot.send_message(telUsers[usraname],message)
        writeLog(logFile,f'message was sent to {telUsers[usraname]}')
    else:
        writeLog(logFile,'error: user not found \nStart conversation with bot')
        bot.polling()

def starTelegramBot():
    global telUsers
    telUsers=readFTD('tusers.txt')
    
bot.polling()

###########################################################################
#                                                                         #
###########################################################################





#send mail to reciver
def writeMail(message, reciver):
    msg=EmailMessage()
    msg.set_content(f'Date: {datetime.now()} \n{message}')

    msg['Subject']='Daemon site watcher'
    msg['From']='spammailbox98@gmail.com'
    msg['To']=reciver

    server=smtplib.SMTP("smtp.gmail.com",587)
    server.starttls()
    server.login("spammailbox98", "c15ef1f9660731187b01435bda747686dbf0e530c3b3a626e7d7c389e56c6ac1")
    server.send_message(msg)
    writeLog(logFile,f'Email was sent to {reciver}')

#function for writing default config of daemon
def createConfig(path):
    config=configparser.ConfigParser()
    config['Settings']={
        'WantedWord': 'Доступ до публічної інформації',
        'WantedNumber':'37-45-70',
        'CheckPeriodSec': '5',
        'Source': 'http://91.202.128.107:55555/6D/LMC/index.html',
        'ShutdownOnHttpError': 'no',
        'ShutdonwOnNetError': 'no',
        'StartCountingOnError' : 'yes',
        'stillAlive': 'yes',
        'WaitGoodOnStart': 'yes',
        'LogPath': 'swd.log',
        'ShutdownCommand':'shutdown -P now'}
    try:
        with open(path, 'w') as file:
            config.write(file)
        return True
    except:
        return False

alreadyExistNumber=[]
newnumbers=[]

def inintNumbers():
    global newnumbers
    pattern="\\d\\d-\\d\\d-\\d\\d"
    result=re.findall(pattern,page.text)
    #writeLog(logFile,result)
    for str in result:
        sum=0
        for char in str:
            if char.isdigit():
                sum=sum+int(char)
        if sum%5==0:  
            newnumbers.append(str)

def findAllNmbers():
    global alreadyExistNumber
    alreadyExistNumber=newnumbers.copy()
    
    newnumbers.clear()
    #writeLog(logFile,f"alreadyExistNumber:{alreadyExistNumber}\nnewnumbers:{newnumbers}")
    pattern="\\d\\d-\\d\\d-\\d\\d"
    result=re.findall(pattern,page.text)
    #writeLog(logFile,result)
    for str in result:
        sum=0
        for char in str:
            if char.isdigit():
                sum=sum+int(char)
        if sum%5==0:
            #writeLog(logFile,f"detected {str}")   
            newnumbers.append(str)
            if str not in alreadyExistNumber:
                #writeLog(logFile,f'found str:{str}')
                return str
    return 'none'


def checkNumber(number):
    global wantedNumber
    if number not in page.text and wantedNumber:
        wantedNumber=False       
        detectedNumber=findAllNmbers()
        if detectedNumber!='none':
            writeMail(f"'{number}' was lost, number {detectedNumber} detected",'triod315@gmail.com')
            broadcastSend(f"'{number}' was lost, number {detectedNumber} detected")
            writeLog(logFile,f"'{number}' was lost in {source}, number {detectedNumber} detected")
    wantedNumber=number in page.text
    

def checkWord(word):
    global page
    global wanted
    if word not in page.text and wanted:
        wanted=False
        #writeMail(f"'{word}' was lost",'triod315@gmail.com')
        print(f"'{word}' was lost")
        broadcastSend(f"'{word}' was lost")
        writeLog(logFile,f"'{word}' was lost in {source}")
    wanted=word in page.text

#########################################################
#                   Main daemon part                    #
#########################################################

configFile='settings.ini'
if not os.path.exists(configFile):
    if not(createConfig(configFile)):
        writeLog('swd.log', 'Error while creating config file')
    else:
        writeLog(logFile,f'config file "{configFile}" was created')
else:
    writeLog(logFile,f'load config from "{configFile}"')

config = configparser.ConfigParser()
config.read(configFile)
settings = config['Settings']
wantedWord=settings.get('WantedWord','text')
wantedNuber=settings.get('WantedNumber','37-45-70')
checkPeriod = settings.getint('CheckPeriodSec', 5)
shutdownOnHTTPError = settings.getboolean('ShutdownOnHTTPError', False)
stillAlive = settings.getboolean('stillAlive', True)
shutdownOnNetError = settings.getboolean('ShutdownOnNetError', False)
startCountingOnError = settings.getboolean('StartCountingOnError', True)
waitOnStart = settings.getboolean('WaitGoodOnStart', True)
logFile = settings.get('LogPath', 'UPSLog.log')
source = settings.get('Source', 'http://91.202.128.107:55555/6D/LMC/index.html')
command = settings.get('ShutdownCommand', 'shutdown -P now')

starTelegramBot()

writeLog(logFile,"daemon has been started")
page=requests.get(source)
inintNumbers()
while True:
    try:
        page=requests.get(source)
        page.encoding='utf-8'
        httpCode=page.status_code
    except requests.exceptions.ConnectionError:
        httpCode=0
    
    if httpCode==0:
        writeLog(logFile,'Connction Error')
        if shutdownOnNetError:
            shutdownAllow=True
        elif stillAlive:
            writeLog(logFile,'Connection fault, still alive...')
            sleep(checkPeriod)
            continue
    elif httpCode==200:       
        checkWord(wantedWord)
        checkNumber(wantedNuber)
    else:
        writeLog(logFile, f"HTTP Error: {httpCode}")
    sleep(checkPeriod)

