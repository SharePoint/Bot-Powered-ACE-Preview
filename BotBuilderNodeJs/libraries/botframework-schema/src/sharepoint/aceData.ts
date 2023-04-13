// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

export enum AceCardSize {
    Medium = "Medium",
    Large = "Large"
}

/**
 * Sharepoint Ace Data object
 */
export class AceData {
    private cardSize: AceCardSize;
    private dataVersion: string;
    private id: string;
    private title: string;
    private iconProperty: string;
    /**
     * Initializes a new instance of the AceData class
     */
    public AceData(){
        // Do nothing
    }

    /**
     * Sets card size property of type AceCardSize
     */
    public set CardSize(cardSize: AceCardSize){
        this.cardSize = cardSize;
    }

    /**
     * Gets card size property of type AceCardSize
     */
    public get CardSize(): AceCardSize {
        return this.cardSize;
    }

    /**
     * Sets data version property of type string
     */
    public set DataVersion(dataVersion: string){
        this.dataVersion = dataVersion;
    }

    /**
     * Gets data version property of type string
     */
    public get DataVersion(): string {
        return this.dataVersion; 
    }

    /**
     * Sets id property of type string
     */
    public set Id(id: string) {
        this.id = id;
    }

    /**
     * Gets id property of type string
     */
    public get Id(): string {
        return this.id;
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

    /**
     * Sets icon property of type string
     */
    public set IconProperty(iconProperty: string){
        this.iconProperty = iconProperty;
    }

    /**
     * Gets icon property of type string
     */
    public get IconProperty(): string{
        return this.iconProperty;
    }
}