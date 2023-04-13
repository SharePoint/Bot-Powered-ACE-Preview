// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IPropertyPaneGroupOrConditionalGroup } from './IPropertyPaneGroupOrConditionalGroup';
import { PropertyPaneField } from './propertyPaneField';

/**
 * Sharepoint PropertyPaneGroup object
 */
export class PropertyPaneGroup implements IPropertyPaneGroupOrConditionalGroup {
    private groupFields: [PropertyPaneField];
    private groupName: string;
    private isCollapsed: boolean;
    private isGroupNameHidden: boolean;
    /**
     * Initializes a new instance of the PropertyPaneGroup class
     */
    public PropertyPaneGroup(){
        // Do nothing
    }

    /**
     * Sets the group fields of type PropertyPaneGroupField
     */
    public set GroupFields(groupFields: [PropertyPaneField]){
        this.groupFields = groupFields;
    }

    /**
     * Gets the group fields of type PropertyPaneGroupField
     */
    public get GroupFields(): [PropertyPaneField] {
        return this.groupFields;
    }

    /**
     * Sets the group name of type string
     */
    public set GroupName(groupName: string){
        this.groupName = groupName;
    }

    /**
     * Gets the group name of type string
     */
    public get GroupName(): string {
        return this.groupName;
    }

    /**
     * Sets a value indicating whether the PropertyPane group is collapsed or not of type boolean
     */
    public set IsCollapsed(isCollapsed: boolean){
        this.isCollapsed = isCollapsed;
    }

    /**
     * Gets a value indicating whether the PropertyPane group is collapsed or not of type boolean
     */
    public get IsCollapsed(): boolean {
        return this.isCollapsed;
    }

    /**
     * Sets a value indicating whether the group name should be hidden of type boolean
     */
     public set IsGroupNameHidden(isGroupNameHidden: boolean){
        this.isGroupNameHidden = isGroupNameHidden;
    }

    /**
     * Gets a value indicating whether the group name should be hidden of type boolean
     */
    public get IsGroupNameHidden(): boolean {
        return this.isGroupNameHidden;
    }

}