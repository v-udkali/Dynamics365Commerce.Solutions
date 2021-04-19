/**
 * SAMPLE CODE NOTICE
 * 
 * THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED,
 * OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.
 * THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.
 * NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
 */

import { GetDeviceConfigurationClientRequest, GetDeviceConfigurationClientResponse } from "PosApi/Consume/Device";
import { ExtensionOperationRequestHandlerBase, ExtensionOperationRequestType } from "PosApi/Create/Operations";
import { ClientEntities } from "PosApi/Entities";
import { ObjectExtensions } from "PosApi/TypeExtensions";
import { Alerts } from "../../../DataService/DataServiceRequests.g";
import AlertListDialog from "../../Views/AlertsDialog";
import QueueAlertRequest from "./QueueAlertRequest";
import QueueAlertResponse from "./QueueAlertResponse";

/**
 * (Sample) Request handler for the QueueAlertRequest class.
 */
export default class QueueAlertHandler<TResponse extends QueueAlertResponse> extends ExtensionOperationRequestHandlerBase<QueueAlertResponse> {
    /**
     * Gets the supported request type.
     * @return {RequestType<TResponse>} The supported request type.
     */
    public supportedRequestType(): ExtensionOperationRequestType<TResponse> {
        return QueueAlertRequest;
    }

    /**
     * Executes the request handler asynchronously.
     * @param {QueueAlertRequest<TResponse>} request The request.
     * @return {Promise<ICancelableDataResult<TResponse>>} The cancelable async result containing the response.
     */
    public executeAsync(request: QueueAlertRequest<TResponse>): Promise<ClientEntities.ICancelableDataResult<QueueAlertResponse>> {

        // Get current store number
        return this.context.runtime.executeAsync(new GetDeviceConfigurationClientRequest(this.context.logger.getNewCorrelationId()))
            .then((response: ClientEntities.ICancelableDataResult<GetDeviceConfigurationClientResponse>): Promise<ClientEntities.ICancelableDataResult<Alerts.GetAlertsByStoreResponse>> => {
                return this.context.runtime.executeAsync(
                    new Alerts.GetAlertsByStoreRequest<Alerts.GetAlertsByStoreResponse>(response.data.result.StoreNumber));
            })
            // get alerts
            .then((result: ClientEntities.ICancelableDataResult<Alerts.GetAlertsByStoreResponse>)
                : Promise<ClientEntities.ICancelableDataResult<QueueAlertResponse>> => {

                if (!ObjectExtensions.isNullOrUndefined(result)
                    && !ObjectExtensions.isNullOrUndefined(result.data)
                    && !result.canceled) {

                    let alertListDialog: AlertListDialog = new AlertListDialog(result.data.result);
                    return alertListDialog.open().then((): Promise<ClientEntities.ICancelableDataResult<QueueAlertResponse>> => {
                        return Promise.resolve(<ClientEntities.ICancelableDataResult<QueueAlertResponse>>{ canceled: false, data: new QueueAlertResponse() });
                    });                    
                }
                else {
                    return Promise.resolve(<ClientEntities.ICancelableDataResult<QueueAlertResponse>>{
                        canceled: true,
                        data: null
                    });
                }
            });
    }
}   
    
    
   