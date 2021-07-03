import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})

export class MemberListComponent implements OnInit {

  members!: Member[];
  pagination: Pagination;
  pageNumber: number = 1;
  pageSize: number = 5;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.pageNumber = 1;
    this.pageSize = 5;
    this.loadMembers();
  }

  loadMembers(){
    this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe(response =>{
      this.members = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any){
    console.log(event)
    console.log(event.page);
    this.pageNumber = event.page;
    this.loadMembers();
  }

}
