import { startOfWeek, endOfWeek, setHours, setMinutes, addMinutes, endOfDay, getDay, format } from 'date-fns';

export interface SelectDateModel {
    date: Date;
    name: string;
}

export const dateSelect = (date?, weekDay?): SelectDateModel[] => {

    const currentDate = new Date();
    const beginingWeek = startOfWeek(currentDate, { weekStartsOn: 1 });
    const endWeek = endOfWeek(currentDate, { weekStartsOn: 1 });
    let changeTime = setMinutes(setHours(currentDate, 0), 0);
    let midnight = setMinutes(setHours(currentDate, 0), 0);
    let data: Date[] = [];


    while (getDay(midnight) === getDay(changeTime)) {
         data.push(changeTime);
         changeTime = addMinutes(changeTime, 15);
    }
    const select = data.map((currentData: Date) => ({
        date: currentData,
        name: format(currentData, 'HH:mm')
    }));

    return select;
};
