import mysql.connector

mydb = mysql.connector.connect(
  host="192.168.254.131",
  user="root",
  passwd="123456",
  database="TradeDB"
)

curs=mydb.cursor()



command=input("Input type(1 - Tradeing_EnterpricesUkraine, 2 - Amount_of_trade, join)")

if command=='1':
    curs.execute("select * from Trading_enterprisessUkraine")
if command=='2':
    curs.execute("select * from Amount_of_trade")
if command=='3':
    curs.execute("select * from Trading_enterprisessUkraine inner join Amount_of_trade on Amount_of_trade.ID_am = Trading_enterprisessUkraine.AmountOfTrade_ID")

reader=curs.fetchall()

for r in reader:
    for j in range(0,len(r)):
        print('%14s' % r[j], end=" ")
    print("\n")
    