import { Files } from "./files";
import { Pictures } from "./pictures";
import { Videos } from "./videos";

export interface Documentation {

  publicId: string;
  title: string;  
  description: string;  
  addedDateTime: string;  


  files: Files[];  
  pictures: Pictures[];  
  videos: Videos[];  

}

