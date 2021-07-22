import pyodbc


print(pyodbc.drivers())

cnxn = pyodbc.connect("DRIVER={ODBC Driver 13 for SQL Server};"
                      "SERVER=DEV-NB-0073\SQLEXPRESS;"
                      "DATABASE=BRevolution;"
                      "Trusted_Connection=yes;")


cursor = cnxn.cursor()
cursor.execute('SELECT * FROM Countries')

for row in cursor:
    print('row = %r' % (row,))