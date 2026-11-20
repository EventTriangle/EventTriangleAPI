import {Injectable, signal} from '@angular/core';

type PaymentForm = 'transfer' | 'deposit';

@Injectable({providedIn: 'root'})
export class PaymentSubmissionService {
  private readonly pending = signal<Partial<Record<PaymentForm, string>>>({});

  isPending(form: PaymentForm): boolean {
    return this.pending()[form] !== undefined;
  }

  start(form: PaymentForm): string | undefined {
    if (this.isPending(form)) return;
    const id = crypto.randomUUID();
    this.pending.update(pending => ({...pending, [form]: id}));
    return id;
  }

  finish(id?: string | null): boolean {
    if (!id) return false;

    const {transfer, deposit} = this.pending();
    if (id === transfer) {
      this.pending.set({deposit});
      return true;
    }
    if (id === deposit) {
      this.pending.set({transfer});
      return true;
    }
    return false;
  }
}
