import { Files } from "./files";
import { Pictures } from "./pictures";
import { Videos } from "./videos";

export interface Documentation {

  id: number;  // Assuming 'id' is the unique identifier for a documentation entry
  title: string;  // The title of the documentation
  description: string;  // The description of the documentation
  addedDateTime: string;  // The date and time the documentation was added

  // Assuming you also have URLs for files, pictures, and videos
  files: Files[];  // Array of files associated with this documentation
  pictures: Pictures[];  // Array of pictures associated with this documentation
  videos: Videos[];  // Array of videos associated with this documentation

}

