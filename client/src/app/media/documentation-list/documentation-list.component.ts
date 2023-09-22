import { Component, Input, Renderer2 } from '@angular/core';
import { Router } from '@angular/router';
import { Documentation } from 'src/app/_models/documentation';
import { DocumentationService } from 'src/app/_services/documentation.service';

@Component({
  selector: 'app-documentation-list',
  templateUrl: './documentation-list.component.html',
  styleUrls: ['./documentation-list.component.scss']
})
export class DocumentationListComponent {

  @Input() documentations: Documentation | undefined;
  documentation: Documentation[] = [];

  currentPage: number = 1;
  itemsPerPage: number = 8;
  totalItems: number;


  constructor(private documentationService: DocumentationService, private router: Router,
    private renderer: Renderer2) { }

  redirectToDetail(id: number) {
    this.router.navigate(['/documentation-detail', id]);
  }


  ngOnInit(): void {
    this.documentationService.getDocumentations().subscribe(documentation => { // Subscribe to the observable here
      this.documentation = documentation;
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

  get paginatedList() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.documentation.slice(startIndex, endIndex);
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }
  
  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }
  
  get totalPages() {
    return Math.ceil(this.documentation.length / this.itemsPerPage);
  }
  


  onRowMouseEnter(event: Event) {
    this.renderer.addClass(event.target, 'hovered-row');
  }

  onRowMouseLeave(event: Event) {
    this.renderer.removeClass(event.target, 'hovered-row');
  }

}
