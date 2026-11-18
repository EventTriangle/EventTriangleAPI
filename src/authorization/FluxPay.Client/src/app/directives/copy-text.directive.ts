import {Directive, HostListener, Input} from '@angular/core';

@Directive({
  selector: '[appCopyText]'
})
export class CopyTextDirective {
  @Input() appCopyText: string = '';

  @HostListener('click') onClick() {
    this.copyToClipboard();
  }

  private copyToClipboard() {
    const textarea = document.createElement('textarea');
    textarea.value = this.appCopyText;
    document.body.appendChild(textarea);
    textarea.select();
    document.execCommand('copy');
    document.body.removeChild(textarea);
  }
}
