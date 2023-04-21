// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint card view parameters
 */
export class BaseCardParameters {
    private iconProperty: string;
    private iconAltText: string;
    private title: string;

    /**
     * Initializes a new instance of the BaseCardParameters class
     */
    public BaseCardParameters(){
        // Do nothing
    }

    /**
     * Sets icon property property of type string
     */
    public set IconProperty(iconProperty: string){
        this.iconProperty = iconProperty;
    }

    /**
     * Gets icon property property of type string
     */
    public get IconProperty(): string {
        return this.iconProperty;
    }

    /**
     * Sets icon alt text property of type string
     */
    public set IconAltText(iconAltText: string){
        this.iconAltText = iconAltText;
    }

    /**
     * Gets icon alt text property of type string
     */
    public get IconAltText(): string {
        return this.iconAltText;
    }

    /**
     * Sets title property of type string
     */
    public set Title(title: string){
        this.title = title;
    }

    /**
     * Gets title property of type string
     */
    public get Title(): string {
        return this.title;
    }
}