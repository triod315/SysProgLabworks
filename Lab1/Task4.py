import sys
import smtplib

from email.message import EmailMessage
from datetime import datetime

if len(sys.argv)<3:
        print('SYNTAX: Task4 <ToAddr> <Subject>')
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
