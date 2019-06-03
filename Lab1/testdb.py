import pyodbc
cnxn = pyodbc.connect(r'Driver={SQL Server Native Client 11.0};Server=.\SQLEXPRESS;Database=TestDatabes;Trusted_Connection=yes;')
cursor = cnxn.cursor()
cursor.execute("SELECT ID,Name FROM Animals")
rows = cursor.fetchall()
print("==========")

width=2
height=len(rows)+1


for row in rows:
    print(row.ID, row.Name)
print("==========")    