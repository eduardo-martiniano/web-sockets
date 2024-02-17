import { Component } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  dictStocks: { [key: string]: { startValue: number; currentValue: number; change: number } } = {
    "ITSA4": {
      startValue: 20.0,
      currentValue: 20.0,
      change: 0.0
    },
    "TAEE11": {
      startValue: 20.0,
      currentValue: 20.0,
      change: 0.0
    },
    "PETR4": {
      startValue: 20.0,
      currentValue: 20.0,
      change: 0.0
    }
  };

  private _hubConnection!: HubConnection;

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  connectToStock(symbol: string) {
    this._hubConnection.invoke('ConnectToStock', symbol);
  }

  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5198/brokerhub')
      .build();
  }

  private startConnection(): void {
    this._hubConnection
      .start()
      .then(() => {
        for (let key in this.dictStocks) {
          this.connectToStock(key);
        }
      })
      .catch(() => {
        // setTimeout(function () { this.startConnection(); }, 5000);
      });
  }

  private registerOnServerEvents(): void {
    this._hubConnection.on('UpdatePrice', (data) => {
      let item = this.dictStocks[data.symbol as string];
      item.currentValue = data.price;
      item.change = (item.currentValue - item.startValue) / item.startValue;
    });
  }
}
