from appJar import gui
import os.path
import sys
TO_ROOT_DIR = '../'
sys.path.append(os.path.join(os.path.dirname(__file__), TO_ROOT_DIR))
from TCN_file_processor.tcnProcessor import processing_TCN, send_data, log_TCN


class TCNapp:
    def __init__(self, API=None):
        self._API = API

        self._app = gui('Automatikus TCN szerkeztő', '540x300')
        self._app.setBg('PapayaWhip')

        self._front = 'front'
        self._sourcePath = None
        self._targetPath = None

        self.make_start_window()
        self._app.go()

    def make_start_window(self):
        self._app.setPadding(5,5)
        self._app.addDirectoryEntry('deS',colspan=3).theButton.config(text="Mappa",bg='Moccasin', fg='black')
        self._app.setEntryBg('deS', 'Khaki')
        self._app.setEntryFg('deS', 'black')
        self._app.addDirectoryEntry('deT',colspan=3).theButton.config(text="Mappa",bg='Moccasin', fg='black')
        self._app.setEntryBg('deT', 'Khaki')
        self._app.setEntryFg('deT', 'black')

        row = self._app.getRow()
        self._app.addMessage('list', self._message_text(105), row=row, column=0, rowspan=2, colspan=1).config(bg='white')
        self._app.setMessageAlign('list','nw')
        self._app.setMessageWidth('list', 105)
        self._app.addEntry('peL', row=row, column=1, colspan=2).config(bg='Khaki', fg='black')
        self._app.addButton('Add', self._add_button_, row=row + 1, column=1).config(text="Hozzáad", bg='Moccasin', fg='black')
        self._app.addButton('Clear', self._clear_button_, row=row + 1, column=2).config(text="Törlés", bg='Moccasin', fg='black')
        row = self._app.getRow()
        self._app.addButton('Convert', self._run_button_, row=row, column=0).config(text='Konvertálás', bg='Moccasin', fg='black')
        self._app.addButton('Cancel',self._app.stop,row=row,column=2).config(text='Mégse',bg='Moccasin', fg='black')

        self._app.setEntryDefault('deS','Forrás mappa')
        if self._sourcePath is not None:
            self._app.setEntry('deS', self._sourcePath)

        self._app.setEntryDefault('deT','Cél mappa')
        if self._targetPath is not None:
            self._app.setEntry('deT', self._targetPath)

        self._app.setEntryDefault('peL', 'Előlap megnevezése')
        # self._app.setEntry('peL', 'front')

    def make_result_window(self, count):
        self._app.setPadding(5,5)
        front = "Előlap: %s" %count['FRONT']
        back = "Hátlap: %s" %count['BACK']
        other = "Egyéb: %s" %count['OTHER']
        self._app.addLabels([front, back, other], colspan=3)
        row = self._app.getRow()
        self._app.addButton('Back', self._back_button_, row=row, column=0).config(text='Vissza', bg='Moccasin', fg='black')
        self._app.addButton('Cancel',self._app.stop,row=row,column=2).config(text='Mégse',bg='Moccasin', fg='black')

    def _back_button_(self, btn):
        self._app.removeAllWidgets()
        self.make_start_window()

    def _add_button_(self, btn):
        txt = self._app.getEntry('peL')
        self._front += '%s%s' %( ';' if len(self._front) != 0 and len(txt) != 0 else '' ,txt)
        self._app.setMessage('list', self._message_text(105))

    def _clear_button_(self, btn):
        self._front = ''
        self._app.setMessage('list', self._front)

    def _run_button_(self, btn):
        source = self._app.getEntry('deS')
        self._sourcePath = source
        target = self._app.getEntry('deT')
        self._targetPath = target
        panel = self._app.getMessage('list')
        try:
            counters = processing_TCN(source, target, panel.split(';'))
            success = send_data(self._API, target, counters)
            log_TCN(success, counters)
            self._app.removeAllWidgets()
            self.make_result_window(counters)
        except FileNotFoundError:
            self._app.warningBox("Olvasási Hiba", "A forrás mappa nem található!")
        except Exception as e:
            log_TCN(False, e)
            self._app.warningBox("ERROR","Nem lekezelt hiba fordult elő!")

    def _message_text(self, length):
        s = ""
        for part in self._front.split(';'):
            if len(s + part) + 1 >= length:
                s += '\n'
            s += part + ";"
        return s[:-1]


if __name__ == "__main__":
    TCNapp()