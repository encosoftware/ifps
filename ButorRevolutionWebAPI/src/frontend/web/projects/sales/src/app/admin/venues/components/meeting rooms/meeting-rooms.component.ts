import { Component, OnInit, Input } from '@angular/core';
import { IHamburgerMenuModel } from 'butor-shared-lib';
import { MatDialog } from '@angular/material/dialog';
import { NewMeetingRoomComponent } from '../new-meeting-room/new-meeting-room.component';
import { IVenueMeetingRoomViewModel } from '../../models/venues.model';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'butor-venue-meeting-rooms',
  templateUrl: './meeting-rooms.component.html',
  styleUrls: ['./meeting-rooms.component.scss']
})
export class VenueMeetingRoomsComponent implements OnInit {

  @Input() meetingRooms: IVenueMeetingRoomViewModel[];

  constructor(
    public dialog: MatDialog,
    private translate: TranslateService,
  ) { }

  ngOnInit() {
  }

  deleteMeetingRoom(itemIndex: number) {
    this.meetingRooms.splice(itemIndex, 1);
  }

  openNewMeetingRoomDialog() {
    const dialogRef = this.dialog.open(NewMeetingRoomComponent, {
      width: '48rem',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.meetingRooms.push(result);
      }
    });
  }

  openEditMeetingRoomDialog(itemIndex: number) {
    const dialogRef = this.dialog.open(NewMeetingRoomComponent, {
      width: '48rem',
      data: {
        data: this.meetingRooms[itemIndex]
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.meetingRooms[itemIndex] = result;
      }
    });
  }

}
