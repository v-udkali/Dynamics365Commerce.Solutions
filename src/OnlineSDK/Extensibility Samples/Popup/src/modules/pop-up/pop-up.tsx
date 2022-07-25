/*--------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * See License.txt in the project root for license information.
 *--------------------------------------------------------------*/

import * as Msdyn365 from '@msdyn365-commerce/core';
import { IModuleProps, INodeProps, Modal, ModalBody, ModalHeader } from '@msdyn365-commerce-modules/utilities';
import classnames from 'classnames';
import { observer } from 'mobx-react';
import * as React from 'react';

import { setPopUpState } from './actions';
import { IPopUpData } from './pop-up.data';
import { IPopUpProps } from './pop-up.props.autogenerated';

/**
 * Pop up view props.
 */
export interface IPopUpViewProps extends IPopUpProps<IPopUpData> {
    moduleProps: IModuleProps;
    headerContainerProps: INodeProps;
    headerProps?: React.ReactNode;
    bodyContainerProps: INodeProps;
    bodyContent: React.ReactNode;
    className: string;
    onDismiss(): void;
}

/**
 *
 * PopUp component.
 * @extends {React.PureComponent<IPopUpProps<IPopUpData>>}
 */
@observer
class PopUp extends React.PureComponent<IPopUpProps<IPopUpData>> {
    public componentDidMount(): void {
        const {
            config: { isCookieEnabled, shouldPopUpOnLoad },
            context: {
                actionContext,
                app: {
                    config: { popUpCookieName }
                }
            }
        } = this.props;

        if (shouldPopUpOnLoad) {
            const popUpState = { isOpen: true };
            const cookieName = (popUpCookieName as string) || '_msdyn365__popUp_';
            if (!isCookieEnabled || !this._isActionTaken(cookieName)) {
                actionContext.update(setPopUpState(popUpState), popUpState);
            }
        }
    }

    public render(): JSX.Element | null {
        const {
            config: { className, heading, isBackDropStatic },
            data,
            slots: { content },
            resources
        } = this.props;

        const isDialogOpen = data.popUpState.result?.isOpen;

        const viewProps = {
            ...this.props,
            onDismiss: this._onDismiss,
            moduleProps: {
                tag: Modal,
                moduleProps: this.props,
                className: classnames('ms-pop-up', className),
                autoFocus: true,
                backdrop: isBackDropStatic ? 'static' : true,
                fade: true,
                isOpen: isDialogOpen,
                'aria-label': heading ?? resources.popUpAriaLabel,
                onClosed: this._onDismiss,
                horizontalPosition: 'center',
                verticalPosition: 'center',
                toggle: this._onDismiss
            },
            headerContainerProps: {
                tag: ModalHeader,
                className: 'ms-pop-up__modal-header',
                toggle: this._onDismiss
            },
            headerProps: heading && (
                <Msdyn365.Text
                    className='ms-pop-up__modal-title'
                    {...heading}
                    tag='h2'
                    text={heading}
                    editProps={{ onEdit: this.handleHeadingChange, requestContext: this.props.context.request }}
                />
            ),
            bodyContainerProps: {
                tag: ModalBody,
                className: 'ms-pop-up__modal-body'
            },
            bodyContent: content.map((item: React.ReactNode, index: number) => this._getPopUpContent(item, index))
        };

        return this.props.renderView(viewProps) as React.ReactElement;
    }

    /**
     * Handle the heading change event.
     * @param event - Content Editable Event.
     */
    public handleHeadingChange = (event: Msdyn365.ContentEditableEvent): void => {
        this.props.config.heading = event.target.value;
    };

    /**
     * _isActionTaken method - To check cookie value.
     * @param cookieName - Cookie name.
     * @returns - Boolean value for existence of cookie or it's value itself.
     */
    private readonly _isActionTaken = (cookieName: string): boolean => {
        const cookie = this.props.context.request.cookies;
        const popUpCookie = cookie.get(cookieName) as Msdyn365.ICookieValue<{}> | undefined;

        if (popUpCookie === undefined || popUpCookie.value === 'false') {
            return false;
        }

        return !!popUpCookie.value;
    };

    /**
     * _onDismiss method - To dismiss pop up.
     */
    private readonly _onDismiss = (): void => {
        const popUpState = { isOpen: false };
        this.props.context.actionContext.update(setPopUpState(popUpState), popUpState);
    };

    /**
     * _getPopUpContent method - To get pop up content.
     * @param item - Pop up item.
     * @param index - Index.
     * @returns - Pop up content as react element.
     */
    private readonly _getPopUpContent = (item: React.ReactNode, index: number): React.ReactNode => {
        return <React.Fragment key={index}>{React.cloneElement(item as React.ReactElement)}</React.Fragment>;
    };
}

export default PopUp;
