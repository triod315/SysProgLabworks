#run in terminal before start:
#$ sudo pip install pytelegrambotapi

import sys
import smtplib
import telebot

from email.message import EmailMessage
from datetime import datetime

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
        print(f"{ln[0]} {ln[1]}\n")
    return dictionary


TeleToken='727963015:AAES9Q_dFuT171PNSbBc_kW6htbT3Y-rf-k'

bot=telebot.TeleBot(TeleToken)

telUsers={}

def addUser(id,username):
    if not id in telUsers:
        telUsers[username]=id
        writeDTF(telUsers,"tusers.txt")
        print(f"id: {id} \t username: {username} added")
            
@bot.message_handler(commands=["start"])
def start(message):
    bot.send_message(message.chat.id, "You have been selected")
    addUser(message.chat.id,message.chat.username)
    bot.stop_polling()

def sendTelegramMessage(message, usraname):
    if usraname in telUsers:
        bot.send_message(telUsers[usraname],message)
        print(f'message was sent {telUsers[usraname]}')
    else:
        print('error: user not found \nStart conversation with bot')
        bot.polling()
        

def starTelegramBot():
    global telUsers
    telUsers=readFTD('tusers.txt')
    

def main():
    try:
        if len(sys.argv) == 4:
            
            starTelegramBot()
            message=f'Subject: {sys.argv[2]}\nDate: {datetime.now()} \nSender: Oleksandr Hryshchuk'
            sendTelegramMessage(message,sys.argv[1])

        else:
            msg=EmailMessage()
            msg.set_content(f'Date: {datetime.now()} \nSender: Oleksandr Hryshchuk')

            msg['Subject']=sys.argv[2]
            msg['From']='spammailbox98@gmail.com'
            msg['To']=sys.argv[1]

            server=smtplib.SMTP("smtp.gmail.com",587)
            server.starttls()
            server.login("spammailbox98", "c15ef1f9660731187b01435bda747686dbf0e530c3b3a626e7d7c389e56c6ac1")
            server.send_message(msg)
    except:
        print('SYNTAX: Task4 <ToAddr> <Subject> [-t]')


if __name__=="__main__":
    main()
