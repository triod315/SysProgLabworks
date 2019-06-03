import sys
import mysql.connector
import tkinter as tk
import tkinter.ttk as ttk

mydb = mysql.connector.connect(
  host="192.168.254.129",
  user="root",
  passwd="123456",
  database="TradeDB"
)

curs=mydb.cursor()




    
##############################################################
#
#                        Essential task
#
##############################################################

def essTask():
    command=input("Input type(1 - Tradeing_EnterpricesUkraine, 2 - Amount_of_trade, join)\n")

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

##############################################################
#
#                           End
#
##############################################################

##############################################################
# 
#                       Visual interface
# 
##############################################################    

#tkinter table  
class Table(tk.Frame):
    def __init__(self, parent=None, headings=tuple(), rows=tuple()):
        super().__init__(parent)
  
        table = ttk.Treeview(self, show="headings", selectmode="browse")
        table["columns"]=headings
        table["displaycolumns"]=headings
  
        for head in headings:
            table.heading(head, text=head, anchor=tk.CENTER)
            table.column(head, anchor=tk.CENTER,width=120)
  
        for row in rows:
            table.insert('', tk.END, values=tuple(row))
  
        scrolltable = tk.Scrollbar(self, command=table.yview)
        table.configure(yscrollcommand=scrolltable.set)
        scrolltable.pack(side=tk.RIGHT, fill=tk.Y)
        table.pack(expand=tk.YES, fill=tk.BOTH)
   
def readEnterprises():
    global table
    curs.execute("select * from Trading_enterprisessUkraine")

    res=[]
    reader=curs.fetchall()
    for r in reader:
        res.append(r)
    table.destroy()
    table = Table(frame1, headings=('ID', 'Enterprise name', 'Owner','Type','Price','Amount_of_trade_id'), rows=res)
    table.pack(expand=tk.YES, fill=tk.BOTH)

def readAmountofTrade():
    global table
    curs.execute("select * from Amount_of_trade")

    res=[]
    reader=curs.fetchall()
    for r in reader:
        res.append(r)
    table.destroy()
    table = Table(frame1, headings=('ID_am', 'Money' , 'Description'), rows=res)
    table.pack(expand=tk.YES, fill=tk.BOTH)

def readJoin():
    global table
    curs.execute("select * from Trading_enterprisessUkraine inner join Amount_of_trade on Amount_of_trade.ID_am = Trading_enterprisessUkraine.AmountOfTrade_ID")

    res=[]
    reader=curs.fetchall()
    for r in reader:
        res.append(r)
    table.destroy()
    table = Table(frame1, headings=('ID', 'Enterprise name', 'Owner','Type','Price','Amount_of_trade_id','ID_am', 'Money' , 'Description'), rows=res)
    table.pack(expand=tk.YES, fill=tk.BOTH)


def visualRun():

    global table

    global frame

    global frame1

    root = tk.Tk()

    frame=tk.Frame(root)
    frame.pack()

    frame1=tk.Frame(root)
    frame1.pack(side=tk.BOTTOM)

    table = Table(frame1, headings=('ID', 'Enterprise name', 'Owner','Type','Price','Amount_of_trade_id'), rows=[])
    table.pack(expand=tk.YES, fill=tk.BOTH)

    bluebutton = tk.Button(frame, text = "Select trading enterprises", command=readEnterprises)
    bluebutton.pack( side = tk.LEFT )

    bluebutton = tk.Button(frame, text = "Select amount of trade", command=readAmountofTrade)
    bluebutton.pack( side = tk.LEFT )

    bluebutton = tk.Button(frame, text = "Joined table", command=readJoin)
    bluebutton.pack( side = tk.LEFT )

    root.mainloop()

##############################################################
#
#                    End visual interface
#
##############################################################


##############################################################
#
#                           CLI mode
#
##############################################################

#print table from Database to console
def printTable(head):
    reader=curs.fetchall()
    for j in range(0,len(head)):
        print('%14s' % head[j], end=" ")
    print("\n")

    for r in reader:
        for j in range(0,len(r)):
            print('%14s' % r[j], end=" ")
        print("\n")

def insertAmount():
    money=input('\nMoney: ')
    description=input('\nDescription: ')    

    query = f"insert into Amount_of_trade(Money, Description) values({money}, N'{description}')"
    curs.execute(query)

    mydb.commit()

#print table amount_of_trade
def showAm():
    curs.execute("select * from Amount_of_trade")   

    printTable(('ID_am', 'Money' , 'Description'))

#print table TradeingEnterpriesessUkraine
def showEnt():
    curs.execute("select * from Trading_enterprisessUkraine")
    printTable(('ID', 'Enterprise name', 'Owner','Type','Price','Amount_of_trade_id'))

def showJoin():
    curs.execute("select * from Trading_enterprisessUkraine inner join Amount_of_trade on Amount_of_trade.ID_am = Trading_enterprisessUkraine.AmountOfTrade_ID")    
    printTable(('ID', 'Enterprise name', 'Owner','Type','Price','Amount_of_trade_id','ID_am', 'Money' , 'Description'))

#delete data from amout of trade
def deleteAm():
    id=input('ID: ')
    query=f"delete from Amount_of_trade where ID_am = {id}"
    curs.execute(query)
    mydb.commit()
    print(f'record id={id} has been deleted successfully\nWARNING: if this record doesn`t exist this record wont be deleted:) ')
    
def deleteEnt():
    id=input("ID: ")
    query= f"delete from Trading_enterprisessUkraine where ID = {id}"
    curs.execute(query)
    mydb.commit()
    print(f'record id={id} has been deleted successfully\nWARNING: if this record doesn`t exist this record wont be deleted:) ')
    
def updatAm():
    id_ent=input("ID: ")
    money=input("Money: ")
    description=input("Description: ")
    query=f"update Amount_of_trade set Money = {money}, Description = n'{description}' where ID_am={id_ent}"
    curs.execute(query)
    mydb.commit()
    print(f'record id={id_ent} has been updated successfully\nWARNING: if this record doesn`t exist this record wont be updated:) ')
    
def updateEnt():
    recordId=input("ID: ")
    orgName=input("Organization name: ")
    orgOwner=input("Org owner: ")
    orgType=input("Type: ")
    price=input("Price: ")
    amountOfTradeID=input("amountOfTradeID: ")

    query=f"update Trading_enterprisessUkraine set EnterpriseName = n'{orgName}', EnterpriseOwner = n'{orgOwner}', Type = n'{orgType}', Price = {price}, AmountOfTrade_ID = {amountOfTradeID} where ID = {recordId}"
    curs.execute(query)
    mydb.commit()
    print(f'record id={id} has been updated successfully\nWARNING: if this record doesn`t exist this record wont be updated:) ')
    
def insertInt():
    orgName=input("Organization name: ")
    orgOwner=input("Org owner: ")
    orgType=input("Type: ")
    price=input("Price: ")
    amountOfTradeID=input("amountOfTradeID: ")

    query=f"insert into Trading_enterprisessUkraine(EnterpriseName, EnterpriseOwner, Type, Price, AmountOfTrade_ID) values(n'{orgName}', n'{orgOwner}', n'{orgType}', {price}, {amountOfTradeID})"
    curs.execute(query)
    mydb.commit()
    print(f'record has been inserted successfully\n')
    

def runCli():
    while True:
        command=input("Task3 CLI > ")
        if command=='help':
            print('''Task3 CLI help:\n
        showam: \tshow Amount_of_trade
        showent: \tshow trading enterprises
        showjoin: \tshow joined table
        --------------------------------------------
        delam: \t\tdelate amount of trade    
        delent: \tdelate trading enterprise

        --------------------------------------------
        updam: \t\tupdate amount of trade
        updent \t\tupdate enterprice

        --------------------------------------------
        insam: \t\tinsert into Amount_of_trade
        insent: \tinsert into enterprises

        --------------------------------------------
        exit: \t\texit to command line

            ''')
        
        if command=='showam':
            showAm()

        if command=='showent':
            showEnt()

        if command=='showjoin':
            showJoin()

        if command=='delam':
            deleteAm()

        if command=='delent':
            deleteEnt()

        if command=='updam':
            updatAm()

        if command=='updent':
            updateEnt()

        if command=='insam':
            insertAmount()

        if command=='insent':
            insertInt()
    
        if command=='exit':
            break

##############################################################
#
#                          End CLI mode
#
##############################################################

if __name__=='__main__':    
    if len(sys.argv)==1:
        essTask()
    elif sys.argv[1]=="-v":
        visualRun()
    elif sys.argv[1]=='-c':
        runCli()