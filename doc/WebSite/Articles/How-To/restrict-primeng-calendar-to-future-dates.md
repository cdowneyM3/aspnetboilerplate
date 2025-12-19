## Introduction

This guide explains how to configure a PrimeNG p-calendar component to only allow future date selection. This is useful for expiration dates, appointment scheduling, or any scenario where past dates should not be selectable.

## Implementation

### Component Template (HTML)

To restrict the calendar to only allow future dates, use the `minDate` property and bind it to today's date:

```html
<p-calendar
    [(ngModel)]="expirationDateModel"
    (ngModelChange)="onExpirationDateChange($event)"
    selectionMode="single"
    [readonlyInput]="true"
    inputId="expirationDate"
    name="expirationDate"
    [numberOfMonths]="1"
    #expirationDateInput="ngModel"
    [showButtonBar]="true"
    [styleClass]="'width-percent-100 mb-3'"
    [placeholder]="l('SelectDate')"
    [minDate]="minDate"
    required
></p-calendar>
```

### Component Class (TypeScript)

In your component class, define the `minDate` property and set it to today's date:

```typescript
import { Component, OnInit } from '@angular/core';

export class YourComponent implements OnInit {
    expirationDateModel: Date;
    minDate: Date;

    ngOnInit(): void {
        // Set minimum date to today
        this.minDate = new Date();
    }

    onExpirationDateChange(event: Date): void {
        // Handle the date change event
        console.log('Selected date:', event);
    }
}
```

## Key Properties

- **`[minDate]="minDate"`**: Sets the minimum selectable date. Any date before this will be disabled in the calendar.
- **`[readonlyInput]="true"`**: Prevents manual date entry, ensuring users can only select dates from the calendar picker.
- **`required`**: Makes the field mandatory (used with Angular forms validation).

## Additional Options

### Allow Selection from Tomorrow Onwards

If you want to exclude today and only allow dates from tomorrow onwards:

```typescript
ngOnInit(): void {
    // Set minimum date to tomorrow
    this.minDate = new Date();
    this.minDate.setDate(this.minDate.getDate() + 1);
}
```

### Set Maximum Date

You can also set a maximum date to create a date range:

```html
<p-calendar
    [(ngModel)]="expirationDateModel"
    [minDate]="minDate"
    [maxDate]="maxDate"
    ...other properties
></p-calendar>
```

```typescript
ngOnInit(): void {
    this.minDate = new Date();
    
    // Set maximum date to one year from now
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear() + 1);
}
```

## References

For more information about PrimeNG Calendar component and its properties, visit:
- [PrimeNG Calendar Documentation](https://primeng.org/calendar)
