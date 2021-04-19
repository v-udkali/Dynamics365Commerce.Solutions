/**
 * SAMPLE CODE NOTICE
 * 
 * THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED,
 * OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.
 * THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.
 * NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
 */

import * as Dialogs from "PosApi/Create/Dialogs";
import { IDataList, DataListInteractionMode } from "PosApi/Consume/Controls";
import { ArrayExtensions, ObjectExtensions } from "PosApi/TypeExtensions";
import { Entities } from "../../DataService/DataServiceEntities.g";

export default class AlertListDialog extends Dialogs.ExtensionTemplatedDialogBase {
    private static readonly DATA_LIST_ELEMENT_ID: string = "Microsoft_AlertsExtension_AlertListDialog_DataList";
    public isItemSelected: () => boolean;
    private resolve: () => void;
    private alertData: Entities.Alert[] = [];
    private dataList: IDataList<Entities.Alert>;
    private selectedItem: Entities.Alert;

    constructor(alertData?: Entities.Alert[])
    {
        super();
        this.alertData = alertData;
        this.isItemSelected = () => !ObjectExtensions.isNullOrUndefined(this.selectedItem);
    }
    public onReady(element: HTMLElement): void {
        
        let dataListElement: HTMLDivElement = element.querySelector("#" + AlertListDialog.DATA_LIST_ELEMENT_ID) as HTMLDivElement;
        this.dataList = this.context.controlFactory.create<Entities.Alert>(
            this.context.logger.getNewCorrelationId(),
            "DataList",
            {
                columns: [
                    {
                        collapseOrder: 3,
                        computeValue: (row: Entities.Alert): string => {
                            return row.Name;
                        },
                        isRightAligned: false,
                        minWidth: 100,
                        ratio: 40,
                        title: "Name"
                    },
                    {
                        collapseOrder: 2,
                        computeValue: (row: Entities.Alert): string => {
                            return row.Description
                        },
                        isRightAligned: false,
                        minWidth: 100,
                        ratio: 30,
                        title: "Description"
                    },
                    {
                        collapseOrder: 1,
                        computeValue: (row: Entities.Alert): string => {
                            return row.CreatedDateTime;
                        },
                        isRightAligned: false,
                        minWidth: 50,
                        ratio: 30,
                        title: "Created datetime"
                    }
                ],
                data: this.alertData,
                interactionMode: DataListInteractionMode.SingleSelect,
            },
            dataListElement);

        this.dataList.addEventListener("SelectionChanged", (eventData: { items: Entities.Alert[] }) => {
            this.selectedItem = ArrayExtensions.firstOrUndefined(eventData.items);
        });
    }

    public open(): Promise<void> {

        let promise: Promise<void> = new Promise<void>((resolve: () => void, reject: (reason: any) => void): void => {
            this.resolve = resolve;
            let option: Dialogs.ITemplatedDialogOptions = {
                title: "Checkout queue alerts",
                onCloseX: this.onCloseX.bind(this),
                button1: {
                    id: "Button1",
                    label: "Create task",
                    isPrimary: true,
                    onClick: this.button1ClickHandler.bind(this)
                },
                button2: {
                    id: "Button2",
                    label: "Acknowledge",
                    isPrimary: false,
                    onClick: this.button2ClickHandler.bind(this)
                }
            };

            this.openDialog(option);
            this.setButtonDisabledState("Button1", false);
            this.setButtonDisabledState("Button2", false);
        });

        return promise;
    }

    public closeDialogClicked(): void {
        this.closeDialog();
        this.resolvePromise("Closed");
    }

 
    private onCloseX(): boolean {
        this.resolvePromise("Closed");
        return true;
    }

    private button1ClickHandler(): boolean {
        this.closeDialog();
        return false;
    }

    private button2ClickHandler(): boolean {
        this.closeDialog();
        return false;
    }

    private resolvePromise(result: string): void {
        if (ObjectExtensions.isFunction(this.resolve)) {
            this.resolve();

            this.resolve = null;
        }
    }
}