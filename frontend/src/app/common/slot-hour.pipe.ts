import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'slotHour'
})
export class SlotHourPipe implements PipeTransform {

  transform(value: string, args?: string): string {
    return value;
  }

}
