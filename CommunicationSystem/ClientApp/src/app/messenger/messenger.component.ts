import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { MessengerDataService } from "./messenge.data.service"
import { AccountDataService } from "../account/account.data.service"
import { ToastService } from "../toast.service"
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Group } from './group';
import { UtilitesService } from '../utilites.service';
import { VideochatDataService } from '../videochat/videochat.data.service';
import { Member } from './member';
import { ErrorHandler } from '../infrastructure/error.handler';
import { Guid } from '../infrastructure/guid';
import { Contact } from './contact';
import { Message } from './message';
import { SendMessage } from './sendmessage';

@Component({
  selector: 'app-messenger',
  templateUrl: './messenger.component.html',
  styleUrls: ['./messenger.component.css', '../app.component.css'],
  providers: [
    MessengerDataService,
    AccountDataService,
    ErrorHandler
  ]
})
export class MessengerComponent implements OnInit {
  search: string = "";
  @ViewChild("fileInput") fileInput: ElementRef = new ElementRef("");
  @ViewChild("fileGroupInput") fileGroupInput: ElementRef = new ElementRef("");
  @ViewChild("messagesArea") messagesArea: ElementRef = new ElementRef("");
  currentImage: string = "";
  isOnGroupImage: boolean = false;
  errors: any = {};
  currentGroup: Group = new Group([]);
  searchMembers: Member[] = [];
  groupMembers: Member[] = [];
  fullMemberList: Member[] = [];
  membersSearch: string = "";
  contacts: Contact[] = [];
  currentContact: Contact = new Contact();
  currentContactRow: number = -1;
  messages: Message[] = [];
  page: number = 0;
  isEdit: boolean = false;
  currentMessage: SendMessage = new SendMessage(Number(localStorage.getItem("CURRENT_COMMUNICATION_ID")));
  userGroupList: any[] = [];
  isOpen: boolean = true;
  canScrollDown: boolean = false;
  uploadingFiles: boolean[] = [];

  constructor(private dataService: MessengerDataService, private toastService: ToastService
    , private modalService: NgbModal, public videochatDataService: VideochatDataService
    , private utilitesService: UtilitesService, private errorHandler: ErrorHandler) { }
  ///////////////////////////////////////////////////////////////USERS///////////////////////////////////////////////////////////////////////////////////////////////////////
  OpenImage(path: string, modal: any) {
    this.currentImage = path;
    this.modalService.open(modal, { size: 'xl' });
  }
  FilesSelected(event: any) {
    var files = <FileList>event.target.files;
    if (File.length > 0) {
      for (var i = 0; i < files.length; i++)
        if (files[i].size >= 209715201) {
          this.toastService.showAlert("Файл превышает лимит в 200 мб");
          return;
        }
      for (var i = 0; i < files.length; i++) {
        this.uploadingFiles.push(true);
        this.dataService.sendFileMessages(this.currentMessage, files[i])
          .subscribe((result: any) => {
            this.messages.push(new Message(result.id, true, this.currentMessage.isGroup,
              new Date(), result.path, result.type, '', ''));
            setTimeout(this.ScrollMessages, 10, this.messagesArea.nativeElement);
            this.currentContact.lastMessageDate = new Date();
            this.currentContact.lastMessageType = result.type;
            this.currentMessage.content = "";
            this.uploadingFiles.pop();
          }, error => this.errorHandler.Handle(error))
      }
    }
  }
  OpenFileInput() {
    this.fileInput.nativeElement.click();
  }
  GetFileName(path: string) {
    return path.split("/").pop();
  }
  OpenUserList() {
    this.isOpen = true;
    this.currentContact = new Contact();
    this.currentContactRow = -1;
  }
  SendMessage() {
    if (this.currentMessage.content != "") {
      if (this.isEdit) {
        this.UpdateMessage();
      } else {
        this.AddMessage();
      }
    }
  }
  AddMessage() {
    this.dataService.sendMessage(this.currentMessage).subscribe((result: any) => {
      this.messages.push(new Message(result, true, this.currentMessage.isGroup,
        new Date(), this.currentMessage.content, 0, '', ''));
      setTimeout(this.ScrollMessages, 10, this.messagesArea.nativeElement);
      this.currentContact.lastMessage = this.currentMessage.content;
      this.currentContact.lastMessageDate = new Date();
      this.currentContact.lastMessageType = 0;
      this.currentMessage.content = "";
    }, error => this.errorHandler.Handle(error));
  }
  UpdateMessage() {
    this.dataService.updateMessage(this.currentMessage.id, this.currentMessage.content).subscribe(result => {
      var index = this.messages.findIndex(x => x.id == this.currentMessage.id);
      if (index != -1) {
        this.messages[index].content = this.currentMessage.content;
      }
      var dto = {
        "id": this.currentMessage.id,
        "isGroup": this.currentContact.isGroup,
        "from": Number(localStorage.getItem("CURRENT_COMMUNICATION_ID")),
        "to": this.currentContact.toId,
        "toGroup": this.currentContact.toGroup,
        "content": this.currentMessage.content,
        "previousMessage": this.messages[this.messages.length - 1]?.content ?? "",
        "previousDate": this.messages[this.messages.length - 1]?.date ?? null,
        "previousType": this.messages[this.messages.length - 1]?.type ?? 0
      };
      this.dataService.hubConnection.invoke("UpdateMessage", dto);
      this.UpdateContactInfo(this.currentContact, dto);
      this.currentMessage.content = "";
      this.currentMessage.id = 0;
      this.isEdit = false;
    }
      , error => this.errorHandler.Handle(error));
  }
  EditMessage(id: number, content: string) {
    this.isEdit = true;
    this.currentMessage.id = id;
    this.currentMessage.content = content;
  }
  CancelEditMessage() {
    this.isEdit = false;
    this.currentMessage.id = 0;
    this.currentMessage.content = "";
  }
  DeleteMessage(id: number) {
    this.dataService.deleteMessage(id).subscribe(result => {
      var index = this.messages.findIndex(x => x.id == id);
      if (index != -1) {
        this.messages.splice(index, 1);
      }
      var dto = {
        "id": id,
        "isGroup": this.currentContact.isGroup,
        "from": Number(localStorage.getItem("CURRENT_COMMUNICATION_ID")),
        "to": this.currentContact.toId,
        "toGroup": this.currentContact.toGroup,
        "previousMessage": this.messages[this.messages.length - 1]?.content ?? "",
        "previousDate": this.messages[this.messages.length - 1]?.date ?? null,
        "previousType": this.messages[this.messages.length - 1]?.type ?? 0
      };
      this.dataService.hubConnection.invoke("DeleteMessage", dto);
      this.UpdateContactInfo(this.currentContact, dto);
    }, error => this.errorHandler.Handle(error));
  }
  ScrollMessages(element: any, scrollTo?: number) {
    if (scrollTo !== undefined) {
      element.scrollTop = element.scrollHeight - scrollTo;
    }
    else
      element.scrollTop = element.scrollHeight;
  }
  GetContacts() {
    this.dataService.getContacts().subscribe((result: any) => {
      this.page = 0;
      this.contacts = result;
    }, error => this.errorHandler.Handle(error))
  }
  GetMessages(contact: Contact, scrollTo?: number) {
    if (!contact.isGroup && contact.toId !== undefined) {
      this.dataService.getContactMessages(contact.toId, this.page).subscribe((result: any) => {
        this.messages.unshift(...result);
        setTimeout(this.ScrollMessages, 10, this.messagesArea.nativeElement, scrollTo);
      }, error => this.errorHandler.Handle(error));
    } else {
      this.dataService.getGroupMessages(contact.toGroup, this.page).subscribe((result: any) => {
        this.messages.unshift(...result);
        setTimeout(this.ScrollMessages, 10, this.messagesArea.nativeElement, scrollTo);
      }, error => this.errorHandler.Handle(error));
    }
  }
  SearchContacts() {
    if (this.search != "") {
      this.dataService.searchContacts(this.search).subscribe((result: any) => {
        this.contacts = result;
        this.currentContact = new Contact();
        this.currentContactRow = -1;
      }, error => this.errorHandler.Handle(error));
    } else {
      this.GetContacts();
    }
  }
  SetContact(contact: Contact, index: number) {
    this.isOpen = false;
    this.canScrollDown = false;
    this.messages = [];
    this.page = 0;
    this.currentContact = contact;
    this.currentContact.notViewedMessages = 0;
    this.currentContact.newMessages = 0;
    this.currentMessage.to = contact.toId;
    this.currentMessage.isGroup = contact.isGroup;
    this.currentMessage.toGroup = contact.toGroup;
    this.currentContactRow = index;
    this.GetMessages(contact);
  }
  ScrollDown() {
    this.ScrollMessages(this.messagesArea.nativeElement);
    this.currentContact.newMessages = 0;
  }
  UpdateContactInfo(contact: any, dto: any) {
    contact.lastMessage = dto.previousMessage;
    contact.lastMessageDate = dto.previousDate;
    contact.lastMessageType = dto.previousType;
  }
  onMessageScroll(event: any) {
    //console.log("Scroll");
    if (this.messagesArea.nativeElement.scrollTop == 0 && this.canScrollDown) {
      this.page++;
      var currentScroll = this.messagesArea.nativeElement.scrollHeight;
      this.GetMessages(this.currentContact, currentScroll);
    }
    if (Math.abs(this.messagesArea.nativeElement.scrollHeight -
      this.messagesArea.nativeElement.scrollTop - this.messagesArea.nativeElement.offsetHeight) < 1) {
      this.canScrollDown = false;
      if (this.currentContact.newMessages > 0)
        this.currentContact.newMessages = 0;
    } else {
      this.canScrollDown = true;
    }
  }
  ///////////////////////////////////////////////////////////////USERS///////////////////////////////////////////////////////////////////////////////////////////////////////

  ///////////////////////////////////////////////////////////////GROUPS///////////////////////////////////////////////////////////////////////////////////////////////////////
  AddGroup(modal: any) {
    this.currentGroup = new Group([]);
    this.membersSearch = "";
    this.searchMembers = [];
    this.groupMembers = [];
    this.fullMemberList = [];
    this.modalService.open(modal);
  }
  EditGroup(id: string, modal: any) {
    this.dataService.getGroup(id).subscribe((data: any) => {
      this.groupMembers = data["members"].map((x: Member) => {
        x.isLinked = true;
        x.isSelected = true;
        return x;
      });
      data["members"] = [];
      this.currentGroup = data;
      this.GetFullMemberList();
      this.modalService.open(modal);
    }, error => this.errorHandler.Handle(error))
  }
  EnterImage() {
    this.isOnGroupImage = true;
  }
  LeaveImage() {
    this.isOnGroupImage = false;
  }
  OpenGroupFileInput() {
    this.fileGroupInput.nativeElement.click();
  }
  GroupFileSelected(event: any) {
    var file = <File>event.target.files[0];
    this.utilitesService.postImage(file).subscribe((response: any) => {
      this.currentGroup.groupImage = response;
    }, error => this.errors = this.errorHandler.Handle(error));
  }
  GetFullMemberList() {
    this.fullMemberList = [...new Map(this.searchMembers.concat(this.groupMembers).map(item =>
      [item.userId, item])).values()];
  }
  ChangeMemberState(member: Member) {
    var index = this.currentGroup.members.findIndex(m => m.userId == member.userId);
    if (member.isLinked !== undefined && member.isLinked) {
      if (index != -1) {
        if (member.isSelected) {
          this.currentGroup.members.splice(index, 1);
        }
      } else {
        if (!member.isSelected) {
          member.state = 3;
          this.currentGroup.members.push(member);
        }
      }
    } else {
      if (member.isSelected) {
        member.state = 1;
        this.groupMembers.unshift(member);
        this.currentGroup.members.push(member);
      } else {
        var subindex = this.groupMembers.findIndex(s => s.userId == member.userId);
        this.groupMembers.splice(subindex, 1);
        this.currentGroup.members.splice(index, 1);
      }
      this.GetFullMemberList();
    }
  }
  SearchMembers() {
    if (this.membersSearch != "") {
      this.dataService.getMembersBySearch(this.membersSearch).subscribe((result: any) => {
        this.searchMembers = result;
        this.GetFullMemberList();
      }, error => this.errorHandler.Handle(error));
    } else {
      this.searchMembers = [];
      this.GetFullMemberList();
    }
  }
  SaveGroup() {
    if (Guid.IsEmpty(this.currentGroup.id)) {
      if (this.currentGroup.members.findIndex(x => x.userId == Number(localStorage.getItem("CURRENT_COMMUNICATION_ID"))) == -1) {
        this.currentGroup.members.push(new Member(Number(localStorage.getItem("CURRENT_COMMUNICATION_ID")), 1, "Я"));
      }
      this.dataService.postGroup(this.currentGroup).subscribe(result => {
        this.modalService.dismissAll();
        this.currentGroup = new Group([]);
        this.GetContacts();
      }, error => this.errors = this.errorHandler.Handle(error));
    } else {
      this.dataService.putGroup(this.currentGroup).subscribe(result => {
        this.modalService.dismissAll();
        this.currentGroup = new Group([]);
        this.GetContacts();
      }, error => this.errors = this.errorHandler.Handle(error));
    }
    this.searchMembers = [];
    this.membersSearch = "";
  }
  ///////////////////////////////////////////////////////////////GROUPS///////////////////////////////////////////////////////////////////////////////////////////////////////
  AddHubListeners() {
    this.dataService.addConnectionListener("UpdateMessage", (dto: any) => {
      var contactIndex = this.contacts.findIndex(x => (x.toId == dto.from && !dto.isGroup)
        || (x.toGroup == dto.toGroup && dto.isGroup));
      if (this.currentContactRow == contactIndex) {
        var message = this.messages.find(x => x.id == dto.id);
        if (message !== undefined) {
          message.content = dto.content;
        }
        this.UpdateContactInfo(this.contacts[contactIndex], dto);
      }
    });
    this.dataService.addConnectionListener("DeleteMessage", (dto: any) => {
      var contactIndex = this.contacts.findIndex(x => (x.toId == dto.from && !dto.isGroup)
        || (x.toGroup == dto.toGroup && dto.isGroup));
      if (this.currentContactRow == contactIndex) {
        var index = this.messages.findIndex(x => x.id == dto.id);
        if (index != -1) {
          this.messages.splice(index, 1);
        }
        this.UpdateContactInfo(this.contacts[contactIndex], dto);
      }
    });
    this.dataService.addConnectionListener("ReceiveMessage", (message: any, sender?: any) => {
      var index = this.contacts.findIndex(x => (x.toId == message.from && !message.isGroup)
        || (x.toGroup == message.toGroup && message.isGroup));
      if (index != -1) {
        this.contacts[index].lastMessage = message.content;
        this.contacts[index].lastMessageDate = new Date();
        this.contacts[index].lastMessageType = message.type;
        if (this.currentContactRow == index) {
          if (!this.currentContact.isGroup) {
            this.messages.push(new Message(message.id, false, false,
              new Date(), message.content, message.type));
          } else {
            this.messages.push(new Message(message.id, false, true, new Date(),
              message.content, message.type, sender.nickName, sender.accountImage));
          }
          if (this.canScrollDown)
            this.currentContact.newMessages += 1;
          else
            setTimeout(this.ScrollMessages, 10, this.messagesArea.nativeElement);
          this.dataService.viewMessage(message.id).subscribe(result => { },
            error => this.errorHandler.Handle(error));
        } else {
          this.contacts[index].notViewedMessages += 1;
        }
      } else {
        if (!message.isGroup)
          this.dataService.getContact(message.from).subscribe((result: any) => {
            if (this.currentContactRow != -1)
              this.currentContactRow++;
            this.contacts.unshift(result);
            result.notViewedMessages = 1;
            result.lastMessage = message.content;
            result.lastMessageDate = new Date();
            result.lastMessageType = message.type;
          }, error => this.errorHandler.Handle(error))
        else
          this.dataService.getGroupContact(message.toGroup).subscribe((result: any) => {            
            if (this.currentContactRow != -1)
              this.currentContactRow++;
            this.contacts.unshift(result);
            result.notViewedMessages = 1;
            result.lastMessage = message.content;
            result.lastMessageDate = new Date();
            result.lastMessageType = message.type;
          }, error => this.errorHandler.Handle(error))
      }
    })
  }
  ngOnInit() {
    this.dataService.startConnection();
    this.AddHubListeners();
    this.GetContacts();
  }
}
