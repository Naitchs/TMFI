export interface GetCertDto {

  id: number;
  title: string;
  certType: string;
  fileName: string;
  filePath: string;
  uploadDate: Date;
  certFiles: HrFileDto[];

}

export interface HrFileDto {

  fileType: string;
  fileName: string;
  filePath: string;
  uploadDate: Date;
  certId: number;
  
}


