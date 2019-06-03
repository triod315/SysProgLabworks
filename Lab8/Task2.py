from zeep import Client
from sys import version_info

if version_info.major == 2:
    from tkinter import Tk, Label, Button, Entry, Frame
elif version_info.major == 3:
    from tkinter import Tk, Label, Button, Entry, Frame

def is_username_free():
    result = client.service.IsLoginFree(username_entry.get())
    is_username_free_result_label.config(text = 'Result: ' + str(result))

client = Client('http://mail.univ.net.ua/plutoniy/Service1.svc?wsdl', port_name='HTTPS-Anon')
client.transport.session.verify = False

root = Tk()
root.title('SOAP Request (Task2)')
root.geometry('430x150')
root.resizable(0, 0)

frame = Frame(root)
frame.place(in_ = root, anchor = "c", relx = .5, rely = .5)
frame.pack()

is_username_free_result_label = Label(frame, text = 'Result 2')


is_username_free_button = Button(frame, text = 'Check username', command = is_username_free)
username_entry = Entry(frame)

is_username_free_button.grid(row = 3)

is_username_free_result_label.grid(row = 4)
username_entry.grid(row = 2, pady = (10, 0))
root.mainloop()