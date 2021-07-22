import { setHours, setMinutes, addMinutes, endOfDay, getDay, format, getHours } from 'date-fns';
import { SelectDateModel } from '../app/shared/models/selectDateModel';

export const dateSelect = (): SelectDateModel[] => {

    const currentDate = new Date();
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

export const shortDateSelect = (): SelectDateModel[] => {

    const currentDate = new Date();
    let changeTime = setMinutes(setHours(currentDate, 8), 0);
    let endTime = setMinutes(setHours(currentDate, 21), 0);
    let data: Date[] = [];


    while (getDay(endTime) === getDay(changeTime) && getHours(endTime) !== getHours(changeTime)) {
         data.push(changeTime);
         changeTime = addMinutes(changeTime, 15);
    }
    data.push(changeTime);
    const select = data.map((currentData: Date) => ({
        date: currentData,
        name: format(currentData, 'HH:mm')
    }));

    return select;
};
