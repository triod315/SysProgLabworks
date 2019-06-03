from flask import Flask, request, json
import os
import sys
import smtplib
from email.mime.text import MIMEText
from email.header import Header
from datetime import datetime
import requests

app = Flask(__name__)
DB_FILE_NAME = '/user/src/app/shared_folder/users.txt'

# curl -X GET localhost:5000/users/IsLoginFree?login=admin2
@app.route('/users/IsLoginFree', methods = ['GET'])
def isLoginFree():
    if not os.path.exists(DB_FILE_NAME):
        return json.dumps(True)

    with open(DB_FILE_NAME) as f:
        content = [x.strip() for x in f.readlines()]

    return json.dumps(request.args.get('login') not in content)

# curl -X POST localhost:5000/users/AddLogin -d "{\"login\": \"admin\"}" -H "Content-Type: application/json; charset=UTF-8"
@app.route('/users/AddLogin', methods = ['POST'])
def addLogin():
    login = str(json.loads(request.data)['login'])

    if os.path.exists(DB_FILE_NAME):
        with open(DB_FILE_NAME) as f:
            content = [x.strip() for x in f.readlines()]
        if login in content:
            return 'This login already exists', 400

    with open(DB_FILE_NAME, 'a' if os.path.exists(DB_FILE_NAME) else 'w') as f:
        f.write(login + '\n')

    return json.dumps(login)

@app.route('/users/checkFile', methods = ['POST'])
def checkFile():   
	source=str(json.loads(request.data)['url'])
    try:
		page=requests.get(source)
		page.encoding='utf-8'
		httpCode=page.status_code
	except:
		httpCode=0 

	return json.dumps(httpCode)    
