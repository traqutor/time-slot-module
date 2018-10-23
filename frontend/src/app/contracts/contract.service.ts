import {Injectable} from '@angular/core';
import {ICommodity} from "../commodities/commodity.model";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {MatSnackBar} from "@angular/material";
import {environment} from "../../environments/environment";
import {IContract} from "./contract.model";

@Injectable({
  providedIn: 'root'
})
export class ContractService {

  private contracts: Array<IContract> = [];
  public contractsChanged: BehaviorSubject<Array<IContract>> = new BehaviorSubject<Array<IContract>>([]);

  private url: string;

  constructor(private http: HttpClient,
              private snackBar: MatSnackBar) {
    this.url = environment.url;
  }

  getContracts() {
    this.http.get(`${this.url}/api/Contracts/GetContracts`)
      .subscribe((res: Array<IContract>) => {
        this.contracts = res;
        this.contractsChanged.next(this.contracts);
      });
  }

  getContractsList() {
    this.http.get(`${this.url}/api/Contracts/GetContractList`)
      .subscribe((res: Array<IContract>) => {
        this.contracts = res;
        this.contractsChanged.next(this.contracts);
      });
  }

  getContractById(contractId: number): Observable<IContract> {
    return this.http.get<IContract>(`${this.url}/api/Contracts/GetContract/${contractId}`);
  }

  putContract(contract: IContract, index: number) {

    this.http.put(`${this.url}/api/Contracts/PutContract`, contract)
      .subscribe((res: IContract) => {

        // in case when entry entity ID is 0 that means Add action
        // else is Edit so the object needs to be replaced in array

        if (contract.id === 0) {

          this.contracts.push(res);
          this.snackBar.open('Contract Added', '', {
            duration: 2000,
          });

        } else {

          this.contracts[index] = res;
          this.snackBar.open('Contract Changed', '', {
            duration: 2000,
          });

        }

        this.contractsChanged.next(this.contracts);
      });
  }

  deleteContract(contractId: number, index: number) {
    this.http.delete(`${this.url}/api/Contracts/DeleteContract/${contractId}`)
      .subscribe(() => {
        this.contracts.splice(index, 1);
        this.contractsChanged.next(this.contracts);
        this.snackBar.open('Contract Deleted!', '', {
          duration: 2000,
        });
      });
  }
}
