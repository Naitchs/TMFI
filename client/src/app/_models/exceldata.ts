import { Excelfile } from "./excelfile";

export interface Exceldata {
 

    publicId: string;
    title: string;
    dateUploaded: Date;
    // excelDataRowList: Exceldatarow[];
    excelFiles: Excelfile;  
}
