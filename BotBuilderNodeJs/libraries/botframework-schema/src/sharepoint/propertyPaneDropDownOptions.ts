// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint PropertyPaneDropDownOption object
 */
export class PropertyPaneDropDownOption {
    private index: number;
    private key: string;
    private text: string;
    private type: PropertyPaneDropDownOption.DropDownOptionType;
    /**
     * Initializes a new instance of the PropertyPaneDropDownOption class
     */
    public PropertyPaneDropDownOption(){
        // Do nothing
    }

    /**
     * Sets index for this option of type number
     */
    public set Index(index: number){
        this.index = index;
    }

    /**
     * Gets index for this option of type number
     */
    public get Index(): number {
        return this.index;
    }

    /**
     * Sets a key to uniquely identify this option of type string
     */
    public set Key(key: string){
        this.key = key;
    }

    /**
     * Gets a key to uniquely identify this option of type string
     */
    public get Key(): string {
        return this.key;
    }

    /**
     * Sets text to render for this option of type string
     */
    public set Text(text: string){
        this.text = text;
    }

    /**
     * Gets text to render for this option of type string
     */
    public get Text(): string {
        return this.text;
    }

    /**
     * Sets the type of option. If omitted, the default is PropertyPaneDropdownMenuItemType.Normal of type DropDownOptionType
     */
    public set Type(type: PropertyPaneDropDownOption.DropDownOptionType){
        this.type = type;
    }

    /**
     * Gets the type of option. If omitted, the default is PropertyPaneDropdownMenuItemType.Normal of type DropDownOptionType
     */
    public get Type(): PropertyPaneDropDownOption.DropDownOptionType {
        return this.type;
    }
}

export namespace PropertyPaneDropDownOption{
    export enum DropDownOptionType{
        // Render normal menu item
        Normal = 0,
        // Render a divider
        Divider = 1,
        // Render menu item as a header
        Header = 2
    }
}