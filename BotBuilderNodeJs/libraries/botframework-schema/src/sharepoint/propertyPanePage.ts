// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IPropertyPaneGroupOrConditionalGroup } from './IPropertyPaneGroupOrConditionalGroup';
import { PropertyPanePageHeader } from './propertyPanePageHeader';

/**
 * Sharepoint PropertyPanePage object
 */
export class PropertyPanePage {
    private displayGroupsAsAccordion: boolean;
    private groups: [IPropertyPaneGroupOrConditionalGroup];
    private header: PropertyPanePageHeader;
    
    /**
     * Initializes a new instance of the PropertyPanePage class
     */
    public PropertyPanePage() {
        // Do nothing
    }

    /**
     * Sets a value indicating whether the groups on the PropertyPanePage 
     * are displayed as accordion or not of type boolean
     */
    public set DisplayGroupsAsAccordion(displayGroupsAsAccordion: boolean) {
        this.displayGroupsAsAccordion = displayGroupsAsAccordion;
    }

    /**
     * Gets a value indicating whether the groups on the PropertyPanePage 
     * are displayed as accordion or not of type boolean
     */
    public get DisplayGroupsAsAccordion(): boolean {
        return this.displayGroupsAsAccordion;
    }


    /**
     * Sets the groups of type IPropertyPaneGroupOrConditionalGroup
     */
    public set Groups(groups: [IPropertyPaneGroupOrConditionalGroup] ) {
        this.groups = groups;
    }

    /**
     * Gets the groups of type IPropertyPaneGroupOrConditionalGroup
     */
    public get Groups(): [IPropertyPaneGroupOrConditionalGroup] {
        return this.groups;
    }

    /**
     * Sets the header for the property pane of type PropertyPanePageHeader
     */
    public set Header(header: PropertyPanePageHeader ) {
        this.header = header;
    }

    /**
     * Gets the header for the property pane of type PropertyPanePageHeader
     */
    public get Header(): PropertyPanePageHeader {
        return this.header;
    }
}