// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

export enum PopupWindowPosition{
    Center = 0,
    RightTop = 1,
    LeftTop = 2,
    RightBottom = 3,
    LeftBottom = 4
}

/**
 * Sharepoint PropertyPaneLinkPopupWindowProperties object
 */
export class PropertyPaneLinkPopupWindowProperties {
    private width: number;
    private height: number;
    private title: string;
    private positionWindowPosition: PopupWindowPosition;
    /**
     * Initializes a new instance of the PropertyPaneLinkPopupWindowProperties class
     */
    public PropertyPaneLinkPopupWindowProperties(){
        // Do nothing
    }

     /**
     * Sets the width of the pop up window of type number
     */
     public set Width(width: number){
        this.width = width;
    }

    /**
     * Gets the width of the pop up window of type number
     */
    public get Width(): number {
        return this.width;
    }

    /**
     * Sets the height of the pop up window of type number
     */
    public set Height(height: number){
        this.height = height;
    }

    /**
     * Gets the height of the pop up window of type number
     */
    public get Height(): number {
        return this.height;
    }

    /**
     * Sets the title of pop up window of type string
     */
    public set Title(title: string){
        this.title = title;
    }

    /**
     * Gets the title of pop up window of type string
     */
    public get Title(): string {
        return this.title;
    }

    /**
     * Sets the position of pop up window type PopupWindowPosition
     */
     public set PositionWindowPosition(positionWindowPosition: PopupWindowPosition){
        this.positionWindowPosition = positionWindowPosition;
    }

    /**
     * Gets the position of pop up window type PopupWindowPosition
     */
    public get PositionWindowPosition(): PopupWindowPosition {
        return this.positionWindowPosition;
    }
}