// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { Action } from './action';

export interface IConfirmationDialog{
    title: string;
    message: string;
}

/**
 * Sharepoint action.submit 
 */
export class SubmitAction extends Action{
    private confirmationDialog: IConfirmationDialog;
    /**
     * Initializes a new instance of the SubmitAction class
     */
    public SubmitAction(){
        // Do nothing
    }

    /**
     * Sets confirmation dialog property of type IConfirmationDialog
     */
    public set ConfirmationDialog (confirmationDialog: IConfirmationDialog){
        this.confirmationDialog = confirmationDialog;
    }

    /**
     * Gets confirmation dialog property of type IConfirmationDialog
     */
    public get ConfirmationDialog(): IConfirmationDialog {
        return this.confirmationDialog; 
    }
}