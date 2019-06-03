from tkinter import *
from tkinter.ttk import *
import pyodbc

class App(Frame):

    def __init__(self, parent):
        Frame.__init__(self, parent)
        self.CreateUI()
        self.LoadTable()
        self.grid(sticky = (N,S,W,E))
        parent.grid_rowconfigure(0, weight = 1)
        parent.grid_columnconfigure(0, weight = 1)

    def CreateUI(self):
        tv = Treeview(self)
        tv['columns'] = ('name')
        tv.heading("#0", text='name', anchor='w')
        tv.column("#0", anchor="w")
        tv.heading('name', text='ID')
        tv.column('name', anchor='center', width=80)

        tv.grid(sticky = (N,S,W,E))
        self.treeview = tv
        self.grid_rowconfigure(0, weight = 1)
        self.grid_columnconfigure(0, weight = 1)

    def LoadTable(self):
        
        cnxn = pyodbc.connect(r'Driver={SQL Server Native Client 11.0};Server=.\SQLEXPRESS;Database=TestDatabes;Trusted_Connection=yes;')
        cursor = cnxn.cursor()
        cursor.execute("SELECT ID,name FROM Animals")
        rows = cursor.fetchall()
    
        for row in rows:
            string=row.name.split()
            self.treeview.insert('','end',text=row.name, values=(row.ID))
            print(row.ID, string)



def loadDB():

    cnxn = pyodbc.connect(r'Driver={SQL Server Native Client 11.0};Server=.\SQLEXPRESS;Database=TestDatabes;Trusted_Connection=yes;')
    cursor = cnxn.cursor()
    cursor.execute("SELECT ID,Name FROM Animals")
    rows = cursor.fetchall()
    print("==========")
    for row in rows:
        print(row.ID, row.name)
    print("==========")   

def main():
    root = Tk()
    App(root)
    root.mainloop()

if __name__ == '__main__':
    main()

