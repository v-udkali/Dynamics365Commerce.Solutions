/*--------------------------------------------------------------
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * See License.txt in the project root for license information.
 *--------------------------------------------------------------*/

import * as Msdyn365 from '@msdyn365-commerce/core';
import { buildMockModuleProps } from '@msdyn365-commerce/core-internal';
import { wrapInResolvedAsyncResult } from '@msdyn365-commerce-modules/retail-actions';
import { mount } from 'enzyme';
import * as React from 'react';
import * as renderer from 'react-test-renderer';

import { IPopUpState } from '../actions';
import PopUp from '../pop-up';
import { IPopUpData } from '../pop-up.data';
import { IPopUpConfig, IPopUpProps, IPopUpResources } from '../pop-up.props.autogenerated';

const mockData: IPopUpData = {
    popUpState: wrapInResolvedAsyncResult({
        isOpen: true
    } as IPopUpState)
};

const mockConfig: IPopUpConfig = {
    heading: 'Subscribe'
};

const mockActions = {};

const mockSlots = {
    content: [<div key='1' />]
};

const mockResources: IPopUpResources = {
    popUpAriaLabel: 'Test'
};

describe('PopUp', () => {
    let moduleProps: IPopUpProps<IPopUpData>;

    it('renders correctly', () => {
        moduleProps = buildMockModuleProps(mockData, mockActions, mockConfig) as IPopUpProps<IPopUpData>;
        moduleProps.slots = mockSlots;
        moduleProps.resources = mockResources;
        const component: renderer.ReactTestRenderer = renderer.create(<PopUp {...moduleProps} />);
        const tree: renderer.ReactTestRendererJSON | renderer.ReactTestRendererJSON[] | null = component.toJSON();
        expect(tree).toMatchSnapshot();
    });

    it('renders correctly with out heading', () => {
        moduleProps = buildMockModuleProps(mockData, mockActions, mockConfig) as IPopUpProps<IPopUpData>;
        moduleProps.slots = mockSlots;
        moduleProps.resources = mockResources;
        moduleProps.config.heading = undefined;
        const component: renderer.ReactTestRenderer = renderer.create(<PopUp {...moduleProps} />);
        const tree: renderer.ReactTestRendererJSON | renderer.ReactTestRendererJSON[] | null = component.toJSON();
        expect(tree).toMatchSnapshot();
    });

    it('handles heading change event', () => {
        moduleProps = buildMockModuleProps(mockData, mockActions, mockConfig) as IPopUpProps<IPopUpData>;
        moduleProps.slots = mockSlots;
        moduleProps.resources = mockResources;
        moduleProps.config.heading = undefined;
        const component = mount(<PopUp {...moduleProps} />);
        const popUpInstance = component.instance() as PopUp;
        const event = { currentTarget: { value: 'foo1' }, target: { value: 'foo' } } as Msdyn365.ContentEditableEvent;

        popUpInstance.handleHeadingChange(event);

        expect.assertions(0);
    });

    it('checks for cookie', () => {
        moduleProps = buildMockModuleProps(mockData, mockActions, mockConfig) as IPopUpProps<IPopUpData>;
        moduleProps.slots = mockSlots;
        moduleProps.resources = mockResources;
        moduleProps.config.heading = undefined;
        const component = mount(<PopUp {...moduleProps} />);
        const popUp = component.instance() as PopUp;

        // @ts-expect-error ignore private method
        const isActionTaken = popUp._isActionTaken('Random_Cookie_Name_XYZ');
        expect(isActionTaken).toBe(false);
    });

    it('returns popUp Content', () => {
        moduleProps = buildMockModuleProps(mockData, mockActions, mockConfig) as IPopUpProps<IPopUpData>;
        moduleProps.slots = mockSlots;
        moduleProps.resources = mockResources;
        moduleProps.config.heading = undefined;
        const component = mount(<PopUp {...moduleProps} />);
        const popUp = component.instance() as PopUp;

        // @ts-expect-error ignore private method
        const item = popUp._getPopUpContent(moduleProps.slots.content[0], 0);
        expect(item).toBeTruthy();
    });
});
