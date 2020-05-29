import { Component, OnInit } from '@angular/core';
import { AuditService } from 'src/app/core/services/audit.service';
import { Configuration } from 'src/app/core/helper/configuration';
import { Audit } from 'src/app/core/models/audit';
import HttpStatusCode from 'src/app/core/helper/http.codes';

@Component({
    selector: 'app-audit-log',
    templateUrl: './audit-log.component.html',
    styleUrls: ['./audit-log.component.css']
})
export class AuditLogComponent implements OnInit {

    auditLogs: Audit[];
    hours = 0;
    auditType = 0;

    constructor(private auditService: AuditService, private configuration: Configuration) {}

    ngOnInit(): void {
        this.getAuditContent();
    }

    getAuditContent() {
        this.auditService.getAuditContent().subscribe(response => {
                if (response.status === HttpStatusCode.OK) {
                    this.auditLogs = response.body;
                }
            },
            error => {
                this.configuration.handleError(error);
            });
    }

    getAuditContentByAuditTypeHourRange() {
        this.auditService.getAuditContentByAuditTypeHourRange(this.auditType, this.hours).subscribe(response => {
                if (response.status === HttpStatusCode.OK) {
                    this.auditLogs = response.body;
                }
            },
            error => {
                this.configuration.handleError(error);
            });
    }
}
