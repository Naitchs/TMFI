import { Excelfile } from "./excelfile";

export interface Exceldata {
 

    publicId: string;
    title: string;
    dateFrom: Date;
    dateTo: Date;
    dateUploaded: Date;
    // excelDataRowList: Exceldatarow[];
    excelFiles: Excelfile;  
}
