import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import config from '@/config/config';

export interface AppState {
    activeDir: string;
    activeMode: string;
    activeTheme: string;
    activeLayout: string;
    isCardShadow: boolean;
    isLayout: string;
    isBorderRadius: number;
    isCollapse: string;
    isLanguage: string;
    isSidebarHover: boolean;
    isMobileSidebar: boolean;
}

const initialState: AppState = {
    activeDir: config.activeDir,
    activeMode: config.activeMode,
    activeTheme: config.activeTheme,
    activeLayout: config.activeLayout,
    isCardShadow: config.isCardShadow,
    isLayout: config.isLayout,
    isBorderRadius: config.isBorderRadius,
    isCollapse: config.isCollapse,
    isLanguage: config.isLanguage,
    isSidebarHover: false,
    isMobileSidebar: false,
};

export const appSlice = createSlice({
    name: 'app',
    initialState,
    reducers: {
        setActiveDir: (state, action: PayloadAction<string>) => { state.activeDir = action.payload; },
        setActiveMode: (state, action: PayloadAction<string>) => { state.activeMode = action.payload; },
        setActiveTheme: (state, action: PayloadAction<string>) => { state.activeTheme = action.payload; },
        setActiveLayout: (state, action: PayloadAction<string>) => { state.activeLayout = action.payload; },
        setIsCardShadow: (state, action: PayloadAction<boolean>) => { state.isCardShadow = action.payload; },
        setIsLayout: (state, action: PayloadAction<string>) => { state.isLayout = action.payload; },
        setIsBorderRadius: (state, action: PayloadAction<number>) => { state.isBorderRadius = action.payload; },
        setIsCollapse: (state, action: PayloadAction<string>) => { state.isCollapse = action.payload; },
        setIsLanguage: (state, action: PayloadAction<string>) => { state.isLanguage = action.payload; },
        setIsSidebarHover: (state, action: PayloadAction<boolean>) => { state.isSidebarHover = action.payload; },
        setIsMobileSidebar: (state, action: PayloadAction<boolean>) => { state.isMobileSidebar = action.payload; },
    },
});

export const {
    setActiveDir,
    setActiveMode,
    setActiveTheme,
    setActiveLayout,
    setIsCardShadow,
    setIsLayout,
    setIsBorderRadius,
    setIsCollapse,
    setIsLanguage,
    setIsSidebarHover,
    setIsMobileSidebar,
} = appSlice.actions;

export default appSlice.reducer;
