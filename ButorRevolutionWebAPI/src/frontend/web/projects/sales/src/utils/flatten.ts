import { omit } from 'ramda';

export const flattenObjArray = (obj: any[], pathBy: string) => {
    return obj.reduce((acc, curr) => {
        acc.push(omit([pathBy], curr));
        if (curr[pathBy]) {
            acc = acc.concat(flattenObjArray(curr[pathBy], pathBy));
        }
        return acc;
    }, []);
};
