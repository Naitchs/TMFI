import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Documentation } from 'src/app/_models/documentation';
import { DocumentationService } from 'src/app/_services/documentation.service';

@Component({
  selector: 'app-documentation-detail',
  templateUrl: './documentation-detail.component.html',
  styleUrls: ['./documentation-detail.component.scss']
})
export class DocumentationDetailComponent implements OnInit{
 


  documentations: Documentation | undefined;
  publicId: string | undefined;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  videoUrl?: string;


  constructor(private documentationService: DocumentationService, private route: ActivatedRoute) { }

  ngOnInit(): void{
    this.route.params.subscribe(params => {
      this.publicId = params['publicId'];;
      if (this.publicId) {
        this.loadDoc(this.publicId);
        console.log(this.loadDoc);
      }
    });
    
    
    this.galleryOptions = [
      { 
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false,
      },
      { 
        "imageSize": "contain",
        "previewCloseOnEsc": true,
        "imageAnimation": "slide",
        "previewZoom": true, 
        "previewRotate": true
      },
      { 
        "breakpoint": 500, 
        "width": "300px", 
        "height": "300px", 
        "thumbnailsColumns": 3 
      },
      { 
        "breakpoint": 300, 
        "width": "100%", 
        "height": "200px", 
        "thumbnailsColumns": 2,
      }
    
    ]

    this.galleryImages = this.getImages();

  }


  getImages(){
    if (!this.documentations) return [];
    const imageUrls = [];
    for(const photo of this.documentations.pictures){
        imageUrls.push({
          small: photo.url,
          medium: photo.url,
          big: photo.url
        })
    }
    return imageUrls;
  }

  loadDoc(publicId: string) {
    this.documentationService.getDocumentation(publicId).subscribe({
      next: documentation => {
        this.documentations = documentation; // Assign to documentationList
        this.galleryImages = this.getImages();
      }
    });
  }

  caps(str: string): string {
    if (!str) return str;
  
    return str
      .toLowerCase()
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  }

  hoveredVideo: any; // Define a property to track the hovered video

onVideoHover(video: any) {
  this.hoveredVideo = video;

  setTimeout(() => {
    this.hoveredVideo = null;
  }, 3000); 
}

  
}
