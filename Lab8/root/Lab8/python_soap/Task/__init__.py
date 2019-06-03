from flask import Flask
from flask_spyne import Spyne
from spyne.protocol.soap import Soap11
from spyne.model.primitive import Unicode, Integer
from spyne.model.complex import Iterable
import os
from datetime import datetime

import sys
import requests


app = Flask(__name__)
spyne = Spyne(app)
DB_FILE_NAME = '/user/src/app/shared_folder/users.txt'

'''
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:som="tns">
	<soapenv:Body>
		<som:isLoginFree>
			<som:login>Andrii</som:login>
		</som:isLoginFree>
	</soapenv:Body>
</soapenv:Envelope>
'''

'''
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:som="tns">
	<soapenv:Body>
		<som:addLogin>
			<som:login>Andrii</som:login>
		</som:addLogin>
	</soapenv:Body>
</soapenv:Envelope>
'''

'''
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:som="tns">
	<soapenv:Body>
		<som:SendEmail>
			<som:subject>Andrii</som:subject>
		</som:SendEmail>
	</soapenv:Body>
</soapenv:Envelope>
'''

class SomeSoapService(spyne.Service):
    __service_url_path__ = '/soap/someservice'
    __in_protocol__ = Soap11(validator='lxml')
    __out_protocol__ = Soap11()

    @spyne.srpc(str, _returns=bool)
    def isLoginFree(login):
        if not os.path.exists(DB_FILE_NAME):
            return True

        with open(DB_FILE_NAME) as f:
            content = [x.strip() for x in f.readlines()]

        return login not in content

    @spyne.srpc(str, _returns=str)
    def addLogin(login):
        if os.path.exists(DB_FILE_NAME):
            with open(DB_FILE_NAME) as f:
                content = [x.strip() for x in f.readlines()]
            if login in content:
                return 'This login already exists'

    @spyne.srpc(str, _returns=str)
    def SendEmail(subject):
        emailFrom = 'julia2015olex@gmail.com'
        emailTo = 'julia2015olex@gmail.com'
        password = 'grandrvua'
        server = smtplib.SMTP('smtp.gmail.com:587')
        
        msg = MIMEText(datetime.now().strftime('%Y-%m-%d %H:%M:%S') + ' :: ' + '\n', 'plain', 'utf-8')
        msg['Subject'] = Header(subject, 'utf-8')

        server.starttls()
        server.ehlo()
        server.login(emailFrom, password)
        server.sendmail(emailFrom, emailTo, msg.as_string())
        server.quit()

        return subject

    @spyne.srpc(str, _returns=str)
    def checkFile(url):   
        source=url
        try:
            page=requests.get(source)
            page.encoding='utf-8'
            httpCode=page.status_code
        except:
            httpCode=0 

        return httpCode

if __name__ == '__main__':
    app.run(host = '0.0.0.0', port=5000)